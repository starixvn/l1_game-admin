// Helper functions to reduce complexity
function formatDateTimeCell(cell) {
    const cellText = cell.text();
    if (!cellText || cellText === '-') {   
        return;
    }
    const date = new Date(cellText);
    if (!Number.isNaN(date.getTime())) {
        cell.text(formatDateTimeToGMT7(date));
    }
}

function buildUrlWithParams(gameId, playerId, pageSize) {
    const url = globalThis.location.pathname;
    const params = [];
    
    if (gameId) {
        params.push(`gameId=${encodeURIComponent(gameId)}`);
    }
    if (playerId) {
        params.push(`playerId=${encodeURIComponent(playerId)}`);
    }
    if (pageSize) {
        params.push(`pageSize=${encodeURIComponent(pageSize)}`);
    }
    params.push('page=1');
    
    return params.length > 0 ? `${url}?${params.join('&')}` : url;
}

function getFilterValues() {
    return {
        gameId: $('#gameIdFilter').val().trim(),
        playerId: $('#playerIdFilter').val().trim(),
        pageSize: $('#pageSizeSelect').val()
    };
}

function createTooltip($element, fullText) {
    $('.custom-tooltip').remove();
    
    const tooltip = $(`<div class="custom-tooltip">${fullText}</div>`);
    $('body').append(tooltip);
    
    const offset = $element.offset();
    tooltip.css({
        'top': `${offset.top + $element.outerHeight() + 5}px`,
        'left': `${offset.left}px`,
        'position': 'absolute',
        'z-index': '9999'
    }).addClass('show');
    
    $element.data('tooltip', tooltip);
}

function removeTooltip($element) {
    const tooltip = $element.data('tooltip');
    if (tooltip) {
        tooltip.removeClass('show');
        setTimeout(function () {
            tooltip.remove();
        }, 200);
    }
}

const playerChainResults = (function ($) {
    return {
        init: function () {
            // Check if table exists and has correct structure
            const table = $('#playerChainResultsTable');
            if (table.length > 0) {
                const headerCells = table.find('thead th').length;
                const firstRowCells = table.find('tbody tr:first td').length;
                
                // Check if first row has colspan (no data row)
                const firstRow = table.find('tbody tr:first');
                const hasColspan = firstRow.find('td[colspan]').length > 0;
                
                // Only initialize DataTables if we have actual data rows (not "No data available")
                if (headerCells === 16 && firstRowCells === 16 && !hasColspan) {
                    // Initialize DataTables with proper configuration
                    table.DataTable({
                        "paging": false,
                        "searching": false,
                        "info": false,
                        "ordering": true,
                        "order": [[8, "desc"]],
                        "columnDefs": [
                            { "orderable": false, "targets": [0, 1, 2, 3, 4] },
                            { "orderable": true, "targets": [5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15] },
                        ],
                        "createdRow": function (row, data, dataIndex) {
                            formatDateTimeCell($(row).find('td:eq(8)'));
                            formatDateTimeCell($(row).find('td:eq(9)'));
                            formatDateTimeCell($(row).find('td:eq(10)'));
                        },
                        "autoWidth": false,
                        "responsive": true
                    });
                }
            }
        }
    }
}(jQuery));

$(document).ready(function () {
    playerChainResults.init();

    // Event handlers
    $('#applyFilters').on('click', function () {
        const filters = getFilterValues();
        globalThis.location.href = buildUrlWithParams(filters.gameId, filters.playerId, filters.pageSize);
    });

    $('#clearFilters').on('click', function () {
        globalThis.location.href = globalThis.location.pathname;
    });

    $('#pageSizeSelect').on('change', function () {
        const filters = getFilterValues();
        globalThis.location.href = buildUrlWithParams(filters.gameId, filters.playerId, filters.pageSize);
    });

    $('#gameIdFilter, #playerIdFilter').on('keypress', function (e) {
        if (e.which === 13) {
            $('#applyFilters').click();
        }
    });

    // Tooltip functionality
    $(document).on('mouseenter', '.truncate-cell', function () {
        const $this = $(this);
        const fullText = $this.attr('data-full-text');
        
        if (fullText) {
            createTooltip($this, fullText);
        }
    });

    $(document).on('mouseleave', '.truncate-cell', function () {
        removeTooltip($(this));
    });
});

function formatDateTimeToGMT7(dateString) {
    if (!dateString || dateString === '' || dateString === '-') {
            return '-';
        }

        const date = new Date(dateString);
        if (Number.isNaN(date.getTime())) {
            return '-';
        }

        const gmt7Date = new Date(date.getTime() + (7 * 60 * 60 * 1000));
        
        return gmt7Date.toISOString().replace('T', ' ').substring(0, 19);
    
}
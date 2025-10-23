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
            const table = $('#playerChainResultsTable');
            if (table.length > 0) {
                const headerCells = table.find('thead th').length;
                const firstRowCells = table.find('tbody tr:first td').length;
                
                const firstRow = table.find('tbody tr:first');
                const hasColspan = firstRow.find('td[colspan]').length > 0;
                
                if (headerCells === 14 && firstRowCells === 14 && !hasColspan) {
                    table.DataTable({
                        "paging": false,
                        "searching": false,
                        "info": false,
                        "ordering": true,
                        "order": [[7, "desc"]],
                        "columnDefs": [
                            { "orderable": false, "targets": [0, 1, 2, 3, 5] },
                            { "orderable": true, "targets": [4, 6, 7, 8, 9, 10, 11, 12, 13] },
                        ],
                        "autoWidth": false,
                        "responsive": false,
                    });
                }
            }
        }
    }
}(jQuery));

$(document).ready(function () {
    playerChainResults.init();
    
    // Synchronize scroll between table and bottom scrollbar
    const tableContainer = $('.table-responsive');
    const bottomScrollbar = $('#bottomScrollbar');
    
    if (tableContainer.length && bottomScrollbar.length) {
        // Sync table scroll to bottom scrollbar
        tableContainer.on('scroll', function() {
            const scrollLeft = $(this).scrollLeft();
            bottomScrollbar.scrollLeft(scrollLeft);
        });
        
        // Sync bottom scrollbar to table scroll
        bottomScrollbar.on('scroll', function() {
            const scrollLeft = $(this).scrollLeft();
            tableContainer.scrollLeft(scrollLeft);
        });
        
        // Update bottom scrollbar width to match table content
        function updateBottomScrollbar() {
            const tableWidth = $('#playerChainResultsTable').outerWidth();
            const containerWidth = tableContainer.width();
            
            if (tableWidth > containerWidth) {
                bottomScrollbar.find('div').css('width', tableWidth + 'px');
                bottomScrollbar.show();
            } else {
                bottomScrollbar.hide();
            }
        }
        
        // Initial update
        updateBottomScrollbar();
        
        // Update on window resize
        $(globalThis).on('resize', updateBottomScrollbar);
    }

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

    $('#exportButton').on('click', function () {
        const filters = getFilterValues();
        const params = [];
        
        if (filters.gameId) {
            params.push(`gameId=${encodeURIComponent(filters.gameId)}`);
        }
        if (filters.playerId) {
            params.push(`playerId=${encodeURIComponent(filters.playerId)}`);
        }
        
        const exportUrl = `/PlayerChainResultsManagement/Export${params.length > 0 ? '?' + params.join('&') : ''}`;
        
        // Use fetch to check if export is successful before re-enabling button
        fetch(exportUrl, {
            method: 'GET',
            headers: {
                'Accept': 'text/csv'
            }
        })
        .then(() => {
            const link = document.createElement('a');
            link.href = exportUrl;
            link.download = `PlayerChainResults_${new Date().toISOString().slice(0, 19).replaceAll(':', '-')}.csv`;
            link.target = '_blank';
            document.body.appendChild(link);
            link.click();
            link.remove();
        })
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

    // Copy button functionality
    $(document).on('click', '.copy-label', function (e) {
        e.preventDefault();
        e.stopPropagation();
        
        const $copyButton = $(this);
        const $parentCell = $copyButton.closest('td');
        const copyValue = $parentCell.attr('data-copy-value');
        
        // Copy to clipboard
        navigator.clipboard.writeText(copyValue).then(function() {
            // Success - change button state
            $copyButton.text('Copied').addClass('copied');
            
            // Reset button after 2 seconds
            setTimeout(function() {
                $copyButton.text('Copy').addClass('copy-label').removeClass('copied');
            }, 2000);
            
        });
    });
});

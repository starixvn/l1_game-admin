// ShootingBlitz Odds Settings JavaScript (RTP Config Management)

var shootingBlitzOdds = (function () {
    var rtpConfigs = [];
    var currentModeCount = 0;
    var gameName = '';

    function init() {
        var rtpConfigsEl = document.getElementById('rtpConfigsData');
        var gameNameEl = document.getElementById('gameName');
        
        if (rtpConfigsEl && rtpConfigsEl.value) {
            try {
                rtpConfigs = JSON.parse(rtpConfigsEl.value);
            } catch (e) {
                rtpConfigs = [];
            }
        }
        
        if (gameNameEl) {
            gameName = gameNameEl.value;
        }
    }

    // RTP Selection Functions
    function applyRtpSelection() {
        var selectedId = document.getElementById('selectedRtpConfig').value;
        
        if (!gameName) {
            showError('Game name not found');
            return;
        }
        
        var data = {
            name: gameName,
            rtpConfigId: selectedId
        };
        
        $.ajax({
            url: '/shooting_blitz_rtp/select',
            method: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(data),
            success: function (result) {
                showSuccess();
                location.reload();
            },
            error: function (xhr) {
                showError('Failed to apply RTP selection: ' + (xhr.responseText || 'Unknown error'));
            }
        });
    }

    // RTP Config CRUD Functions
    function showCreateRtpModal() {
        document.getElementById('rtpConfigModalTitle').textContent = 'Create RTP Config';
        document.getElementById('rtpConfigId').value = '';
        document.getElementById('rtpValue').value = '0.95';
        document.getElementById('multiplierStepsTabs').innerHTML = '';
        document.getElementById('multiplierStepsContent').innerHTML = '';
        currentModeCount = 0;
        
        $('#rtpConfigModal').modal('show');
    }

    function addMultiplierMode(modeData) {
        currentModeCount++;
        var modeNum = modeData ? modeData.mode : currentModeCount;
        var tabId = 'mode-' + currentModeCount;
        
        // Add tab
        var tabsContainer = document.getElementById('multiplierStepsTabs');
        var isFirst = tabsContainer.children.length === 0;
        var tabHtml = '<li class="nav-item">' +
            '<a class="nav-link' + (isFirst ? ' active' : '') + '" data-toggle="tab" href="#' + tabId + '">Mode ' + modeNum + '</a>' +
            '</li>';
        tabsContainer.insertAdjacentHTML('beforeend', tabHtml);
        
        // Add tab content
        var contentContainer = document.getElementById('multiplierStepsContent');
        var contentHtml = '<div class="tab-pane' + (isFirst ? ' active' : '') + '" id="' + tabId + '" data-mode="' + modeNum + '">' +
            '<div class="form-group row">' +
            '<div class="col-4">' +
            '<label>Mode Number</label>' +
            '<input type="number" class="form-control mode-number" value="' + modeNum + '" />' +
            '</div>' +
            '<div class="col-8">' +
            '<label>Name</label>' +
            '<input type="text" class="form-control mode-name" value="' + (modeData ? modeData.name : '') + '" />' +
            '</div>' +
            '</div>' +
            '<div class="form-group row">' +
            '<div class="col-4">' +
            '<label>Star Player Rate</label>' +
            '<input type="number" step="0.01" class="form-control star-player-rate" value="' + (modeData ? modeData.starPlayerRate : '0.02') + '" />' +
            '</div>' +
            '<div class="col-8">' +
            '<label>Star Position (comma separated)</label>' +
            '<input type="text" class="form-control star-position" value="' + (modeData && modeData.starPosition ? modeData.starPosition.join(',') : '') + '" />' +
            '</div>' +
            '</div>' +
            '<fieldset class="border p-2">' +
            '<legend class="w-auto">Pays Table (index+1 = picks)</legend>' +
            '<div class="pays-table-container"></div>' +
            '<button type="button" class="btn btn-secondary btn-sm" onclick="shootingBlitzOdds.addPaysRow(this)"><i class="icon-plus"></i> Add Pays</button>' +
            '</fieldset>' +
            '<button type="button" class="btn btn-danger btn-sm mt-2" onclick="shootingBlitzOdds.removeMode(this)"><i class="icon-trash"></i> Remove Mode</button>' +
            '</div>';
        contentContainer.insertAdjacentHTML('beforeend', contentHtml);
        
        // Add pays rows if data provided
        if (modeData && modeData.pays) {
            var tableContainer = contentContainer.lastElementChild.querySelector('.pays-table-container');
            modeData.pays.forEach(function (paysValue, index) {
                addPaysRowToContainer(tableContainer, index + 1, paysValue);
            });
        }
    }

    function addPaysRow(btn) {
        var container = btn.previousElementSibling;
        var currentRows = container.querySelectorAll('.pays-row').length;
        addPaysRowToContainer(container, currentRows + 1, '');
    }

    function addPaysRowToContainer(container, picks, pays) {
        var html = '<div class="form-group row pays-row">' +
            '<div class="col-4">' +
            '<input type="number" class="form-control picks-display" value="' + picks + '" readonly disabled />' +
            '<small class="text-muted">Picks (auto)</small>' +
            '</div>' +
            '<div class="col-4">' +
            '<input type="number" step="0.01" class="form-control pays-input" placeholder="Pays" value="' + pays + '" />' +
            '</div>' +
            '<div class="col-4">' +
            '<button type="button" class="btn btn-danger btn-sm" onclick="shootingBlitzOdds.removePaysRow(this)"><i class="icon-trash"></i></button>' +
            '</div>' +
            '</div>';
        container.insertAdjacentHTML('beforeend', html);
    }

    function removePaysRow(btn) {
        var container = btn.closest('.pays-table-container');
        btn.closest('.pays-row').remove();
        // Re-number the picks
        var rows = container.querySelectorAll('.pays-row');
        rows.forEach(function (row, index) {
            row.querySelector('.picks-display').value = index + 1;
        });
    }

    function removeMode(btn) {
        var tabPane = btn.closest('.tab-pane');
        var tabId = tabPane.id;
        var tabLink = document.querySelector('a[href="#' + tabId + '"]');
        
        if (tabLink) {
            tabLink.closest('li').remove();
        }
        tabPane.remove();
        
        // Activate first remaining tab
        var firstTab = document.querySelector('#multiplierStepsTabs .nav-link');
        var firstContent = document.querySelector('#multiplierStepsContent .tab-pane');
        if (firstTab && firstContent) {
            firstTab.classList.add('active');
            firstContent.classList.add('active');
        }
    }

    function editRtpConfig(id) {
        var config = rtpConfigs.find(function (c) { return c.id === id; });
        if (!config) return;
        
        document.getElementById('rtpConfigModalTitle').textContent = 'Edit RTP Config';
        document.getElementById('rtpConfigId').value = id;
        document.getElementById('rtpValue').value = config.rtp;
        document.getElementById('multiplierStepsTabs').innerHTML = '';
        document.getElementById('multiplierStepsContent').innerHTML = '';
        currentModeCount = 0;
        
        // Load multiplier steps
        if (config.multiplierSteps) {
            config.multiplierSteps.forEach(function (step) {
                addMultiplierMode(step);
            });
        }
        
        $('#rtpConfigModal').modal('show');
    }

    function saveRtpConfig() {
        var id = document.getElementById('rtpConfigId').value;
        var rtp = parseFloat(document.getElementById('rtpValue').value);
        
        // Collect multiplier steps
        var multiplierSteps = [];
        document.querySelectorAll('#multiplierStepsContent .tab-pane').forEach(function (pane) {
            var mode = parseInt(pane.querySelector('.mode-number').value);
            var name = pane.querySelector('.mode-name').value;
            var starPlayerRate = parseFloat(pane.querySelector('.star-player-rate').value);
            var starPositionStr = pane.querySelector('.star-position').value;
            var starPosition = starPositionStr ? starPositionStr.split(',').map(function (x) { return parseInt(x.trim()); }) : [];
            
            // Collect pays as array (index+1 = picks)
            var pays = [];
            pane.querySelectorAll('.pays-row').forEach(function (row) {
                var paysValue = parseFloat(row.querySelector('.pays-input').value);
                if (!isNaN(paysValue)) {
                    pays.push(paysValue);
                }
            });
            
            multiplierSteps.push({
                mode: mode,
                name: name,
                starPlayerRate: starPlayerRate,
                starPosition: starPosition,
                pays: pays
            });
        });
        
        var data = {
            rtp: rtp,
            multiplierSteps: multiplierSteps
        };
        
        var url = id ? '/shooting_blitz_rtp/' + id : '/shooting_blitz_rtp';
        var method = id ? 'PUT' : 'POST';
        
        $.ajax({
            url: url,
            method: method,
            contentType: 'application/json',
            data: JSON.stringify(data),
            success: function (result) {
                $('#rtpConfigModal').modal('hide');
                showSuccess();
                location.reload();
            },
            error: function (xhr) {
                showError('Failed to save RTP config: ' + (xhr.responseText || 'Unknown error'));
            }
        });
    }

    function deleteRtpConfig(id) {
        if (!confirm('Are you sure you want to delete this RTP config?')) return;
        
        $.ajax({
            url: '/shooting_blitz_rtp/' + id,
            method: 'DELETE',
            success: function () {
                showSuccess();
                location.reload();
            },
            error: function (xhr) {
                showError('Failed to delete RTP config: ' + (xhr.responseText || 'Unknown error'));
            }
        });
    }

    function showSuccess() {
        var successMsg = document.getElementById('successMsg');
        if (successMsg) {
            successMsg.classList.remove('hidden-cls');
            setTimeout(function () {
                successMsg.classList.add('hidden-cls');
            }, 3000);
        }
    }

    function showError(message) {
        var errorMsg = document.getElementById('failedMsg');
        if (errorMsg) {
            errorMsg.querySelector('span').textContent = message || 'Error';
            errorMsg.classList.remove('hidden-cls');
            setTimeout(function () {
                errorMsg.classList.add('hidden-cls');
            }, 5000);
        }
    }

    // Initialize on document ready
    $(document).ready(function () {
        init();
    });

    return {
        applyRtpSelection: applyRtpSelection,
        showCreateRtpModal: showCreateRtpModal,
        addMultiplierMode: addMultiplierMode,
        addPaysRow: addPaysRow,
        removePaysRow: removePaysRow,
        removeMode: removeMode,
        editRtpConfig: editRtpConfig,
        saveRtpConfig: saveRtpConfig,
        deleteRtpConfig: deleteRtpConfig
    };
})();

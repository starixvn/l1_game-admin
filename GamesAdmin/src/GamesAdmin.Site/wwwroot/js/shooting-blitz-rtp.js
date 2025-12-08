// ShootingBlitz RTP Tab JavaScript (RTP Config Management)
// Uses unified endpoint /shooting_blitz_config + ActionType

var shootingBlitzRtp = (function () {
    var rtpConfigs = [];
    var currentModeCount = 0;
    var gameName = '';

    // ActionType enum (corresponds to backend)
    var ActionType = {
        UpdateConfig: 0,
        QueryRtpConfigs: 1,
        CreateRtpConfig: 2,
        UpdateRtpConfig: 3,
        DeleteRtpConfig: 4,
        SelectRtpConfig: 5
    };

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
            actionType: ActionType.SelectRtpConfig,
            name: gameName,
            rtpConfigId: selectedId
        };
        
        $.ajax({
            url: '/shooting_blitz_config',
            method: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(data),
            success: function (result) {
                if (result.success) {
                    showSuccess();
                    location.reload();
                } else {
                    showError(result.message || 'Failed to apply RTP selection');
                }
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
        document.getElementById('rtpConfigName').value = '';
        document.getElementById('multiplierStepsTabs').innerHTML = '';
        document.getElementById('multiplierStepsContent').innerHTML = '';
        currentModeCount = 0;
        
        $('#rtpConfigModal').modal('show');
    }

    function addMultiplierMode(modeData) {
        currentModeCount++;
        var opponentCount = modeData ? modeData.opponentCount : currentModeCount;
        var tabId = 'mode-' + currentModeCount;
        
        // Extract home/away data
        var homeData = modeData && modeData.home ? modeData.home : { starPlayerRate: 0.02, startPosition: 3, endPosition: 5 };
        var awayData = modeData && modeData.away ? modeData.away : { starPlayerRate: 0.02, startPosition: 3, endPosition: 5 };
        
        // Add tab
        var tabsContainer = document.getElementById('multiplierStepsTabs');
        var isFirst = tabsContainer.children.length === 0;
        var tabHtml = '<li class="nav-item">' +
            '<a class="nav-link' + (isFirst ? ' active' : '') + '" data-toggle="tab" href="#' + tabId + '">Opponent ' + opponentCount + '</a>' +
            '</li>';
        tabsContainer.insertAdjacentHTML('beforeend', tabHtml);
        
        // Add tab content
        var contentContainer = document.getElementById('multiplierStepsContent');
        var contentHtml = '<div class="tab-pane' + (isFirst ? ' active' : '') + '" id="' + tabId + '" data-opponent-count="' + opponentCount + '">' +
            '<div class="form-group row">' +
            '<div class="col-4">' +
            '<label>Opponent Count</label>' +
            '<input type="number" class="form-control opponent-count" value="' + opponentCount + '" />' +
            '</div>' +
            '<div class="col-8">' +
            '<label>Name</label>' +
            '<input type="text" class="form-control mode-name" value="' + (modeData ? modeData.name : '') + '" />' +
            '</div>' +
            '</div>' +
            // Home Star Player Settings
            '<fieldset class="border p-2 mb-2">' +
            '<legend class="w-auto">Home Star Player Settings</legend>' +
            '<div class="form-group row">' +
            '<div class="col-4">' +
            '<label>Star Player Rate</label>' +
            '<input type="number" step="0.001" class="form-control home-star-player-rate" value="' + homeData.starPlayerRate + '" />' +
            '</div>' +
            '<div class="col-4">' +
            '<label>Start Position</label>' +
            '<input type="number" class="form-control home-start-position" value="' + (homeData.startPosition || 3) + '" />' +
            '</div>' +
            '<div class="col-4">' +
            '<label>End Position</label>' +
            '<input type="number" class="form-control home-end-position" value="' + (homeData.endPosition || 5) + '" />' +
            '</div>' +
            '</div>' +
            '</fieldset>' +
            // Away Star Player Settings
            '<fieldset class="border p-2 mb-2">' +
            '<legend class="w-auto">Away Star Player Settings</legend>' +
            '<div class="form-group row">' +
            '<div class="col-4">' +
            '<label>Star Player Rate</label>' +
            '<input type="number" step="0.001" class="form-control away-star-player-rate" value="' + awayData.starPlayerRate + '" />' +
            '</div>' +
            '<div class="col-4">' +
            '<label>Start Position</label>' +
            '<input type="number" class="form-control away-start-position" value="' + (awayData.startPosition || 3) + '" />' +
            '</div>' +
            '<div class="col-4">' +
            '<label>End Position</label>' +
            '<input type="number" class="form-control away-end-position" value="' + (awayData.endPosition || 5) + '" />' +
            '</div>' +
            '</div>' +
            '</fieldset>' +
            // Pays Table
            '<fieldset class="border p-2">' +
            '<legend class="w-auto">Pays Table (index+1 = picks)</legend>' +
            '<div class="pays-table-container"></div>' +
            '<button type="button" class="btn btn-secondary btn-sm" onclick="shootingBlitzRtp.addPaysRow(this)"><i class="icon-plus"></i> Add Pays</button>' +
            '</fieldset>' +
            '<button type="button" class="btn btn-danger btn-sm mt-2" onclick="shootingBlitzRtp.removeMode(this)"><i class="icon-trash"></i> Remove Mode</button>' +
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
            '<button type="button" class="btn btn-danger btn-sm" onclick="shootingBlitzRtp.removePaysRow(this)"><i class="icon-trash"></i></button>' +
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
        document.getElementById('rtpConfigName').value = config.name || '';
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
        var rtpConfigName = document.getElementById('rtpConfigName').value;
        
        // Collect multiplier steps
        var multiplierSteps = [];
        document.querySelectorAll('#multiplierStepsContent .tab-pane').forEach(function (pane) {
            var opponentCount = parseInt(pane.querySelector('.opponent-count').value);
            var name = pane.querySelector('.mode-name').value;
            
            // Home star player settings
            var homeStarPlayerRate = parseFloat(pane.querySelector('.home-star-player-rate').value);
            var homeStartPosition = parseInt(pane.querySelector('.home-start-position').value) || 3;
            var homeEndPosition = parseInt(pane.querySelector('.home-end-position').value) || 5;
            
            // Away star player settings
            var awayStarPlayerRate = parseFloat(pane.querySelector('.away-star-player-rate').value);
            var awayStartPosition = parseInt(pane.querySelector('.away-start-position').value) || 3;
            var awayEndPosition = parseInt(pane.querySelector('.away-end-position').value) || 5;
            
            // Collect pays as array (index+1 = picks)
            var pays = [];
            pane.querySelectorAll('.pays-row').forEach(function (row) {
                var paysValue = parseFloat(row.querySelector('.pays-input').value);
                if (!isNaN(paysValue)) {
                    pays.push(paysValue);
                }
            });
            
            multiplierSteps.push({
                opponentCount: opponentCount,
                name: name,
                home: {
                    starPlayerRate: homeStarPlayerRate,
                    startPosition: homeStartPosition,
                    endPosition: homeEndPosition
                },
                away: {
                    starPlayerRate: awayStarPlayerRate,
                    startPosition: awayStartPosition,
                    endPosition: awayEndPosition
                },
                pays: pays
            });
        });
        
        var data = {
            actionType: id ? ActionType.UpdateRtpConfig : ActionType.CreateRtpConfig,
            name: gameName, // 傳入遊戲名稱以設置 GameName 字段
            rtpConfigId: id || null,
            rtpConfigName: rtpConfigName,
            multiplierSteps: multiplierSteps
        };
        
        $.ajax({
            url: '/shooting_blitz_config',
            method: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(data),
            success: function (result) {
                if (result.success) {
                    $('#rtpConfigModal').modal('hide');
                    showSuccess();
                    location.reload();
                } else {
                    showError(result.message || 'Failed to save RTP config');
                }
            },
            error: function (xhr) {
                showError('Failed to save RTP config: ' + (xhr.responseText || 'Unknown error'));
            }
        });
    }

    function deleteRtpConfig(id) {
        if (!confirm('Are you sure you want to delete this RTP config?')) return;
        
        var data = {
            actionType: ActionType.DeleteRtpConfig,
            rtpConfigId: id
        };
        
        $.ajax({
            url: '/shooting_blitz_config',
            method: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(data),
            success: function (result) {
                if (result.success) {
                    showSuccess();
                    location.reload();
                } else {
                    showError(result.message || 'Failed to delete RTP config');
                }
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


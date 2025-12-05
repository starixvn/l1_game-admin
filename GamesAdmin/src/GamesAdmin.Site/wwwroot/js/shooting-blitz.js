// ShootingBlitz Game Settings JavaScript

var shootingBlitz = (function () {
    var rtpConfigs = [];
    var teams = [];
    var currentModeCount = 0;

    function init() {
        var rtpConfigsEl = document.getElementById('rtpConfigsData');
        var teamsDataEl = document.getElementById('teamsData');
        
        if (rtpConfigsEl && rtpConfigsEl.value) {
            try {
                rtpConfigs = JSON.parse(rtpConfigsEl.value);
            } catch (e) {
                rtpConfigs = [];
            }
        }
        
        if (teamsDataEl && teamsDataEl.value) {
            try {
                teams = JSON.parse(teamsDataEl.value);
            } catch (e) {
                teams = [];
            }
        }
    }

    function toggleSettingTabs(tabName) {
        var tabs = ['general', 'rtpconfig', 'teams'];
        tabs.forEach(function (tab) {
            var tabElement = document.getElementById(tab);
            var tabHeader = document.getElementById(tab + '-tab-header');
            if (tabElement) {
                tabElement.style.display = tab === tabName ? 'block' : 'none';
            }
            if (tabHeader) {
                tabHeader.classList.toggle('active', tab === tabName);
            }
        });
    }

    // RTP Config Functions
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
            '<button type="button" class="btn btn-secondary btn-sm" onclick="shootingBlitz.addPaysRow(this)"><i class="icon-plus"></i> Add Pays</button>' +
            '</fieldset>' +
            '<button type="button" class="btn btn-danger btn-sm mt-2" onclick="shootingBlitz.removeMode(this)"><i class="icon-trash"></i> Remove Mode</button>' +
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
            '<button type="button" class="btn btn-danger btn-sm" onclick="shootingBlitz.removePaysRow(this)"><i class="icon-trash"></i></button>' +
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
            name: rtpConfigName,
            multiplierSteps: multiplierSteps
        };
        
        var url = id ? '/api/shooting_blitz_rtp/' + id : '/api/shooting_blitz_rtp';
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
            url: '/api/shooting_blitz_rtp/' + id,
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

    // Team Functions
    function showCreateTeamModal() {
        document.getElementById('teamModalTitle').textContent = 'Create Team';
        document.getElementById('teamDbId').value = '';
        document.getElementById('teamId').value = '';
        document.getElementById('teamName').value = '';
        document.getElementById('playerListContainer').innerHTML = '';
        
        $('#teamModal').modal('show');
    }

    function addPlayerRow(name, playerNumber) {
        var container = document.getElementById('playerListContainer');
        var html = '<div class="form-group row player-row">' +
            '<div class="col-5">' +
            '<input type="text" class="form-control player-name" placeholder="Player Name" value="' + (name || '') + '" />' +
            '</div>' +
            '<div class="col-4">' +
            '<input type="number" class="form-control player-number" placeholder="Jersey Number" value="' + (playerNumber || '') + '" />' +
            '</div>' +
            '<div class="col-3">' +
            '<button type="button" class="btn btn-danger btn-sm" onclick="this.closest(\'.player-row\').remove()"><i class="icon-trash"></i></button>' +
            '</div>' +
            '</div>';
        container.insertAdjacentHTML('beforeend', html);
    }

    function editTeam(id) {
        var team = teams.find(function (t) { return t.id === id; });
        if (!team) return;
        
        document.getElementById('teamModalTitle').textContent = 'Edit Team';
        document.getElementById('teamDbId').value = id;
        document.getElementById('teamId').value = team.teamId;
        document.getElementById('teamName').value = team.name;
        document.getElementById('playerListContainer').innerHTML = '';
        
        if (team.playerList) {
            team.playerList.forEach(function (player) {
                addPlayerRow(player.name, player.playerNumber);
            });
        }
        
        $('#teamModal').modal('show');
    }

    function saveTeam() {
        var dbId = document.getElementById('teamDbId').value;
        var teamId = parseInt(document.getElementById('teamId').value);
        var teamName = document.getElementById('teamName').value;
        
        if (!teamId || !teamName) {
            showError('Team ID and Name are required');
            return;
        }
        
        var playerList = [];
        document.querySelectorAll('.player-row').forEach(function (row) {
            var name = row.querySelector('.player-name').value;
            var playerNumber = parseInt(row.querySelector('.player-number').value);
            if (name && !isNaN(playerNumber)) {
                playerList.push({ name: name, playerNumber: playerNumber });
            }
        });
        
        var data = {
            teamId: teamId,
            name: teamName,
            playerList: playerList
        };
        
        var url = dbId ? '/api/shooting_blitz_team/' + dbId : '/api/shooting_blitz_team';
        var method = dbId ? 'PUT' : 'POST';
        
        $.ajax({
            url: url,
            method: method,
            contentType: 'application/json',
            data: JSON.stringify(data),
            success: function (result) {
                $('#teamModal').modal('hide');
                showSuccess();
                location.reload();
            },
            error: function (xhr) {
                showError('Failed to save team: ' + (xhr.responseText || 'Unknown error'));
            }
        });
    }

    function deleteTeam(id) {
        if (!confirm('Are you sure you want to delete this team?')) return;
        
        $.ajax({
            url: '/api/shooting_blitz_team/' + id,
            method: 'DELETE',
            success: function () {
                showSuccess();
                location.reload();
            },
            error: function (xhr) {
                showError('Failed to delete team: ' + (xhr.responseText || 'Unknown error'));
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
        toggleSettingTabs: toggleSettingTabs,
        showCreateRtpModal: showCreateRtpModal,
        addMultiplierMode: addMultiplierMode,
        addPaysRow: addPaysRow,
        removePaysRow: removePaysRow,
        removeMode: removeMode,
        editRtpConfig: editRtpConfig,
        saveRtpConfig: saveRtpConfig,
        deleteRtpConfig: deleteRtpConfig,
        showCreateTeamModal: showCreateTeamModal,
        addPlayerRow: addPlayerRow,
        editTeam: editTeam,
        saveTeam: saveTeam,
        deleteTeam: deleteTeam
    };
})();


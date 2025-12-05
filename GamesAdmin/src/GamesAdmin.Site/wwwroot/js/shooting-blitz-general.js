// ShootingBlitz General Settings JavaScript (Teams Management)
// Uses unified endpoint /shooting_blitz_config + ActionType

var shootingBlitz = (function () {
    var teams = [];

    // ActionType enum (corresponds to backend)
    var ActionType = {
        UpdateConfig: 0,
        QueryRtpConfigs: 1,
        CreateRtpConfig: 2,
        UpdateRtpConfig: 3,
        DeleteRtpConfig: 4,
        SelectRtpConfig: 5,
        QueryTeams: 6,
        CreateTeam: 7,
        UpdateTeam: 8,
        DeleteTeam: 9
    };

    function init() {
        var teamsDataEl = document.getElementById('teamsData');
        
        if (teamsDataEl && teamsDataEl.value) {
            try {
                teams = JSON.parse(teamsDataEl.value);
            } catch (e) {
                teams = [];
            }
        }
    }

    function toggleSettingTabs(tabName) {
        var tabs = ['general', 'teams', 'rtp'];
        tabs.forEach(function (tab) {
            // RTP tab uses 'rtp-tab-content' to avoid ID conflicts
            var elementId = tab === 'rtp' ? 'rtp-tab-content' : tab;
            var tabElement = document.getElementById(elementId);
            var tabHeader = document.getElementById(tab + '-tab-header');
            if (tabElement) {
                tabElement.style.display = tab === tabName ? 'block' : 'none';
            }
            if (tabHeader) {
                tabHeader.classList.toggle('active', tab === tabName);
            }
        });
    }

    // Team Functions
    function showCreateTeamModal() {
        document.getElementById('teamModalTitle').textContent = 'Create Team';
        document.getElementById('teamDbId').value = '';
        document.getElementById('teamId').value = '';
        document.getElementById('teamName').value = '';
        document.getElementById('teamIsEnabled').checked = true;  // Default enabled
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
        document.getElementById('teamIsEnabled').checked = team.isEnabled !== false;  // null/undefined treated as true
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
        var isEnabled = document.getElementById('teamIsEnabled').checked;
        
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
        
        // Use unified endpoint with ActionType (per Cursor Rules 4.2)
        var data = {
            actionType: dbId ? ActionType.UpdateTeam : ActionType.CreateTeam,
            teamDbId: dbId || null,
            teamId: teamId,
            teamName: teamName,
            isEnabled: isEnabled,
            playerList: playerList
        };
        
        $.ajax({
            url: '/shooting_blitz_config',
            method: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(data),
            success: function (result) {
                if (result.success) {
                    $('#teamModal').modal('hide');
                    showSuccess();
                    location.reload();
                } else {
                    showError(result.message || 'Failed to save team');
                }
            },
            error: function (xhr) {
                showError('Failed to save team: ' + (xhr.responseText || 'Unknown error'));
            }
        });
    }

    function deleteTeam(id) {
        if (!confirm('Are you sure you want to delete this team?')) return;
        
        // Use unified endpoint with ActionType (per Cursor Rules 4.2)
        var data = {
            actionType: ActionType.DeleteTeam,
            teamDbId: id
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
                    showError(result.message || 'Failed to delete team');
                }
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
        showCreateTeamModal: showCreateTeamModal,
        addPlayerRow: addPlayerRow,
        editTeam: editTeam,
        saveTeam: saveTeam,
        deleteTeam: deleteTeam
    };
})();


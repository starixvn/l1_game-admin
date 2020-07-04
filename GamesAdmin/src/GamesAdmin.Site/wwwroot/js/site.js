// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

class Sidebar {
    collapseExpandSidebar() {
        let isMobile = window.matchMedia("only screen and (max-device-width: 767px)").matches;

        if (!isMobile) {
            $('#sidebarCollapse').on('click', function () {
                $('#sidebar').toggleClass('active');
            });
        } else {
            $('#sidebarCollapse').on('click', function () {
                $('#sidebar').toggleClass('expand-mobile');
            });
        }
    }

    activeLink() {
        var current = location.pathname;
        $('.vertical-nav-menu li a').each(function () {
            var $this = $(this);

            // we check comparison between current page and attribute redirection.
            if ($this.attr('href') === current) {
                $this.addClass('active');
            }
        });
    }
}

function showModal(selector) {
    $(selector).modal('show');
}

let sideBar = new Sidebar();
sideBar.collapseExpandSidebar();
sideBar.activeLink();
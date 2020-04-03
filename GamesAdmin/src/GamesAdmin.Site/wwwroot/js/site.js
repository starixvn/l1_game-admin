// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

class Sidebar {
    collapseExpandSidebar() {
        $('#sidebarCollapse').on('click', function () {
            $('#sidebar').toggleClass('active');
        });
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

let sideBar = new Sidebar();
sideBar.collapseExpandSidebar();
sideBar.activeLink();
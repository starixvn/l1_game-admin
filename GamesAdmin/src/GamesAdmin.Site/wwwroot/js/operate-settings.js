/**
 * Operate Settings JavaScript
 * Handles tab switching and iframe loading for the operate settings page
 */

/**
 * Switch between tabs and load iframe content
 * @param {string} tabId - The ID of the tab to switch to
 */
function switchTab(tabId) {
    // Remove active class from all tabs and panes
    document.querySelectorAll(".nav-link").forEach((tab) => {
        tab.classList.remove("active");
    });
    document.querySelectorAll(".tab-pane").forEach((pane) => {
        pane.classList.remove("show", "active");
    });

    // Add active class to selected tab and pane
    document.getElementById(tabId + "-tab").classList.add("active");
    const selectedPane = document.getElementById(tabId);
    selectedPane.classList.add("show", "active");

    // Load iframe content if not already loaded
    const iframe = selectedPane.querySelector("iframe");
    if (!iframe.src) {
        const url = iframe.getAttribute("data-url");
        if (url) {
            iframe.src = url;
        }
    }
}

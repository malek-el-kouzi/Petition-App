// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function copyToClipboard(text) {
    navigator.clipboard.writeText(text).then(function () {
        // Show notification
        alert('Petition URL copied to clipboard!');
    }, function (err) {
        console.error('Could not copy text: ', err);
    });
}

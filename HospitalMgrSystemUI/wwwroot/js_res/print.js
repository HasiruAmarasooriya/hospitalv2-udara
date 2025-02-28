// print.js
function logToConsole(message) {
    console.log(message);
}

document.addEventListener("DOMContentLoaded", function () {
    // Attach a click event handler to the "Print" button
    var btnPrint = document.getElementById("btnPrint");
    if (btnPrint) {
        btnPrint.addEventListener("click", function (event) {
            event.preventDefault(); // Prevent the default behavior of the link
            logToConsole("Printing...");
            window.location.href = this.href; // Redirect to the PrintRecept action
        });
    }
});

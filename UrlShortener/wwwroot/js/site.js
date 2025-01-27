// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

document.addEventListener("DOMContentLoaded", function () {
    // Ensure the logout button exists before adding event listener
    var logoutButton = document.getElementById("logoutButton");
    
    if (logoutButton) {
        logoutButton.addEventListener("click", function () {
            // Create a form element to send the POST request
            var form = document.createElement("form");
            form.method = "POST";
            form.action = "/Account/Logout";  // This will go to the Logout action in the Account controller

            //anti-forgery token
            var antiForgeryToken = document.querySelector('input[name="__RequestVerificationToken"]');
            if (antiForgeryToken) {
                var tokenInput = document.createElement("input");
                tokenInput.type = "hidden";
                tokenInput.name = "__RequestVerificationToken";
                tokenInput.value = antiForgeryToken.value;
                form.appendChild(tokenInput);
            }

            // Submit the form
            document.body.appendChild(form);
            form.submit();
        });
    }
});

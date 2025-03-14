// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
document.addEventListener("DOMContentLoaded", function () {
    document.querySelectorAll(".btn-edit").forEach(button => {
        button.addEventListener("click", function () {
            let participantDiv = this.closest(".participant");
            participantDiv.querySelector(".email-text").style.display = "none";
            participantDiv.querySelector(".btn-edit").style.display = "none";
            participantDiv.querySelector(".edit-form").style.display = "flex";
            participantDiv.querySelector(".email-input").style.display = "inline-block";
        });
    });

    document.querySelectorAll(".btn-cancel").forEach(button => {
        button.addEventListener("click", function () {
            let participantDiv = this.closest(".participant");
            participantDiv.querySelector(".email-text").style.display = "inline";
            participantDiv.querySelector(".btn-edit").style.display = "inline";
            participantDiv.querySelector(".edit-form").style.display = "none";
        });
    });
});

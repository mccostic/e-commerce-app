// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function changeValue(isIncrement, inputId) {
    var inputField = document.getElementById(inputId);
    var currentValue = parseInt(inputField.value) || 0; // Get current value, default to 0 if not a number

    if (isIncrement) {
        currentValue += 1; // Increment the value
    } else {
        if (currentValue > 0) {
            currentValue -= 1; // Decrement the value, preventing it from going below 0
        }
    }

    inputField.value = currentValue; // Update the input field with the new value
}

// Dropdown initialization
$(document).ready(function () {
    $('.dropdown-toggle').dropdown();
});
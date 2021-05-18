// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// src/hrmApp/Views/Shared/Components/DataSheetDocuments/Default.cshtml használja
$(function () {
    bsCustomFileInput.init();
});


$(".alert").fadeTo(2000, 500).slideUp(500, function(){
    $(".alert").slideUp(500);
});


function sendPost(element, id, txt) {

    event.preventDefault();
    // Get some values from elements on the page:    
    var url = element.getAttribute("href");
    console.log(url);
    
    // Send the data using post
    var posting = $.ajax({
        type: "POST",        
        url: url,
        headers: {
            "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val()
        },       
        data: {
            id: id,
            EmployeeId: id,
            Text: txt
        }
    });

};

function readyFn( jQuery ) {
    // Code to run when the document is ready.
    console.log("Document ready from readyFN");
}
 
$( document ).ready( readyFn );


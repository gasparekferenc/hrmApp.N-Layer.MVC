function downloadAttachment(element) {

    event.preventDefault();
    // Get some values from elements on the page:    
    var url = element.getAttribute("href");
    console.log(url);

    // Send the data using post
    var posting = $.ajax({
        type: "GET",
        url: url,
        headers: {
            "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val()
        },
        xhrFields: {
            responseType: 'blob' // to avoid binary data being mangled on charset conversion
        },
        success: function(blob, status, xhr) {

    // Get the raw header string
    var headers = xhr.getAllResponseHeaders();
    console.log("headers: " + headers);


    // Convert the header string into an array
    // of individual headers
    var arr = headers.trim().split(/[\r\n]+/);
    console.log("arr: " + arr);
    console.log("arr[1]: " + arr[1]);



            // check for a filename
            var filename = "";
            var disposition = xhr.getResponseHeader('Content-Disposition');
            console.log("disposition: " + disposition);
            if (disposition && disposition.indexOf('attachment') !== -1) {
                var filenameRegex = /filename[^;=\n]*=((['"]).*?\2|[^;\n]*)/;
                var matches = filenameRegex.exec(disposition);
                
                if (matches != null && matches[1]) filename = matches[1].replace(/['"]/g, '');
            }
            console.log("filename: " + filename);

            if (typeof window.navigator.msSaveBlob !== 'undefined') {
                // IE workaround for "HTML7007: One or more blob URLs were revoked by closing the blob for which they were created. These URLs will no longer resolve as the data backing the URL has been freed."
                window.navigator.msSaveBlob(blob, filename);
            } else {
                var URL = window.URL || window.webkitURL;
                var downloadUrl = URL.createObjectURL(blob);
                if (filename) {
                    // use HTML5 a[download] attribute to specify filename
                    var a = document.createElement("a");
                    // safari doesn't support this yet
                    if (typeof a.download === 'undefined') {
                        window.location.href = downloadUrl;
                    } else {
                        a.href = downloadUrl;
                        a.download = filename;
                        document.body.appendChild(a);
                        a.click();
                    }
                } else {
                    window.location.href = downloadUrl;
                }
    
                setTimeout(function () { URL.revokeObjectURL(downloadUrl); }, 100); // cleanup
            }
        }

    });

}



function downloadAttachment2(element)
//function downloadFile(urlToSend)
{
    event.preventDefault();
    // Get some values from elements on the page:    
    var urlToSend = element.getAttribute("href");
    console.log(urlToSend);

    var req = new XMLHttpRequest();
    req.open("GET", urlToSend, true);
    req.setRequestHeader("RequestVerificationToken", $('input[name="__RequestVerificationToken"]').val());
    req.responseType = "blob";
    req.onload = function (event) {
        var blob = req.response;
        var fileName = req.getResponseHeader("filename"); //if you have the fileName header available
        console.log(fileName);

        var fn = "download_file.pdf";
        var link = document.createElement('a');
        link.href = window.URL.createObjectURL(blob);
        // link.download = fileName;
        link.download = fn;
        link.click();
    };

    req.send();
}

$(function () {
    $("#dialog").dialog({
        autoOpen: false,
        show: {
            effect: "size",
            duration: 250
        },
        hide: {
            effect: "size",
            duration: 250
        },
        position: {
            my: "left center",  //"left top-10%",
            at: "center center"  //"left bottom",
            //of: "#myId" 
        },
        maxHeight: 1200,
        width: 800,
        height: 1000
    });
});


function openPreview(element) {

    event.preventDefault();
    // Get some values from elements on the page:    
    var url = element.getAttribute("href");

    var posting = $.ajax({
        type: "GET",
        url: url,
        headers: {
            "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val()
        },
        xhrFields: {
            responseType: 'blob' // to avoid binary data being mangled on charset conversion
        },
        success: function (blob, status, xhr) {

            var URL = window.URL || window.webkitURL;
            var downloadUrl = URL.createObjectURL(blob);
            console.log("downloadUrl: " + downloadUrl);


            var options = {                
                fallbackLink: "<p>A PDF nem jeleníthető meg!</p>",
                pdfOpenParams: {
                    view: 'FitV'
                }
            };

            PDFObject.embed(downloadUrl, "#dialog", options);

            $("#dialog").dialog("open");
        }
    });

};


$( "#deleteConfirm" ).dialog({
    autoOpen: false
  });

function deleteAttachment(element) {

    $( "#deleteConfirm" ).dialog({
        position: {
            my: "right buttom",  //"left top-10%",
            at: "right top",  //"left bottom",
            of: element 
        },
        autoOpen: true
        //show: { effect: "blind", duration: 800 }
    });

    event.preventDefault();
    // Get some values from elements on the page:    
    var url = element.getAttribute("href");
    $( "#deleteConfirm" ).dialog({        
        resizable: false,
        height: "auto",
        width: 400,
        modal: true,
        buttons: {
          "Törlés": function() {
            var posting = $.ajax({
                type: "POST",
                url: url,
                headers: {
                    "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val()
                },
                success: function(){                    
                        window.location.reload();
                }                
            });
            $( this ).dialog( "close" );
          },
          "Mégsem": function() {
            $( this ).dialog( "close" );
          }
        }
    });
};
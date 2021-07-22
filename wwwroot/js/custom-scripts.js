/**
 * Shows a generic alert.
 * @param {any} alertTitle Title shown on alert.
 * @param {any} alertContent Text shown on alert.
 * @param {any} alertType Alert type, accepted values: error, information, warning.
 */
function ShowAlert(alertTitle, alertContent, alertType) {
    var alertBorder = '';
    var buttonClass = '';
    var alertIcon = 'fas fa-exclamation-circle';

    // Default values
    alertTitle = alertTitle == '' ? 'Alert' : alertTitle;
    alertType = alertType == '' ? 'error' : alertType;

    switch (alertType) {
        case 'error':
            alertBorder = 'red';
            buttonClass = 'btn-red';
            alertIcon = 'fas fa-times';
            break;

        case 'information':
            alertBorder = 'blue';
            buttonClass = 'btn-blue';
            alertIcon = 'fas fa-info-circle';
            break;

        case 'warning':
            alertBorder = 'orange';
            buttonClass = 'btn-orange';
            alertIcon = 'fas fa-exclamation-triangle';
            break;

        default:
            break;
    }

    $.alert({
        theme: 'bootstrap',
        icon: alertIcon,
        type: alertBorder,
        title: alertTitle,
        animation: 'zoom',
        closeAnimation: 'scale',
        content: alertContent,
        buttons: {
            okButton: {
                text: 'Ok',
                btnClass: buttonClass
            }
        }
    });
}

/**
 * Refresh content from a given div by id.
 * @param {any} urlAction Url action.
 * @param {any} httpVerb Http verb.
 * @param {any} formData Form id for data serialize (if required).
 * @param {any} refreshId Id from div to refresh its content.
 * @param {any} executeFunction Function to execute before request is done.
 */
function RefreshContent(urlAction, httpVerb, formData, refreshId, executeFunction) {
    $.ajax({
        url: urlAction,
        type: httpVerb,
        data: $(formData).serialize(),
        statusCode: {
            // Correct
            200: function (result) {
                $(refreshId).html(result);
                $(refreshId).show(250);

                // Executes a function after request completes.
                if (executeFunction != '') {
                    executeFunction();
                }
            },
            // NoContent
            204: function (result) {
                ShowAlert('Warning', result.value, 'warning');
            },
            // Error
            500: function (error) {
                ShowAlert('Error', 'Sorry, something went wrong.', 'error');
            }
        }
    });
}

/**
 * Shows screen loader
 * @param {number} timeSpan Seconds until loader hides itself automatically.
 */
function ShowLoader(timeSpan) {
    // Mostrar animación loader
    $.LoadingOverlay("show", {
        background: "rgba(55,51,56,0.8)",
        image: "/img/loader.gif",
        imageAnimation: 'none',
        size: 60, //75
        maxSize: 85, //150
        minSize: 20,
    });

    // Esconder animación loader
    if (timeSpan > 0) {
        HideLoader(timeSpan);
    }
}

/**
 * Hides manually screen loader
 * @param {number} timeSpan Seconds until loader hides itself automatically.
 */
function HideLoader(timeSpan) {
    setTimeout(function () {
        $.LoadingOverlay("hide");
    }, timeSpan);
}

/**
 * Export an html table to CSV format on a new file.
 * @param {string} table_id Id from the table to export as CSV.
 * @param {string} fileName Name of the report to export.
 */
function ExportToCsv(tableId, fileName) {
    var data = [];

    $('#' + tableId + ' tr').each(function () {
        data.push($(this));
    });

    csv_data = []

    data.forEach(function (item, index) {
        td = item[0].children
        for (i = 0; i < td.length; i++) {

            csv_data.push(td[i].innerText)
        }

        csv_data.push('\r\n')
    })

    DownloadFile(csv_data, fileName + '.csv');
}

/**
 * Export an html table to JSON format on a new file.
 * @param {string} tableId Id from the table to export as CSV.
 * @param {string} fileName Name of the report to export.
 */
function ExportToJson(tableId, fileName) {
    var json = '{';
    var otArr = [];
    var tbl2 = $('#' + tableId + ' tr').each(function (i) {
        x = $(this).children();
        var itArr = [];
        x.each(function () {
            itArr.push('"' + $(this).text() + '"');
        });
        otArr.push('"' + i + '": [' + itArr.join(',') + ']');
    })
    json += otArr.join(",") + '}'

    DownloadFile(json, fileName + '.json');
}

/**
 * Export an html table to JSON format on a new file.
 * @param {string} tableId Id from the table to export as CSV.
 * @param {string} fileName Name of the report to export.
 */
function ExportToXml(tableId, fileName)
{
    var xml = "";

    $('#' + tableId + ' tr').each(function () {
        var cells = $('td', this);
        if (cells.length > 0) {
            xml += '<data name="' + cells.eq(0).text() + '">\n';
            for (var i = 1; i < cells.length; ++i) {
                xml += '\t<report>' + cells.eq(i).text() + '</report>\n';
            }
            xml += '</data>\n';
        }
    });

    DownloadFile(xml, fileName + '.xml');
}

/**
 * Creates a downloadable file from given data and given name (including extension)
 * @param {any} data Data for fill out report.
 * @param {any} fileName Name of the file for download.
 */
function DownloadFile(data, fileName) {
    var downloadLink = document.createElement('a');
    var blob = new Blob(['\ufeff', data]);
    var url = URL.createObjectURL(blob);
    downloadLink.href = url;
    downloadLink.download = fileName;
    document.body.appendChild(downloadLink);
    downloadLink.click();
    document.body.removeChild(downloadLink);
}
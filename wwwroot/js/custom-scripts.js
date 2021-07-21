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

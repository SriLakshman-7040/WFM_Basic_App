let candidateID = null;
let employeeManager = null;
let requestMessage = null;
let empManagerStatus = null;

$(document).ready(function () {

    $("#candidateListTbl").on('click', '#candidateDetailsModal', showSoftLockModal); 
})

function showSoftLockModal() {
    let currentRow = $(this).closest("tr");
    candidateID = currentRow.find(".employeeid").text();    
    employeeManager = currentRow.find(".employeeManager").text();
    empManagerStatus = "Awaiting_Confirmation"    
    
    $("#staticBackdrop").modal('show');
    document.getElementById('showCandidateId').innerHTML = candidateID;    
    $("#softLockReq").click(function () {
        $("#staticBackdrop").modal('hide');
        $('.modal-backdrop').remove();
        // call a post req to SoftLock Table        
        candidateLockReq();
    });
    $("#softLockCancel").click(function () {
        $("#showCandidateId").empty();       
        $('#reqMessage').val(null);
    })

}

function candidateLockReq() {
    requestMessage = $('textarea[name="requestMsg"]').val(); 
    const params = {
        employeeId: candidateID.trim(),
        manager: employeeManager.trim(),
        requestMessage: requestMessage,
        managerStatus: empManagerStatus
    }

    const requestOptions = {
        method: 'POST',
        headers: { 'Content-Type': 'application/json', "XSRF-TOKEN": $('input[name="__RequestVerificationToken"]').val()}, //
        body: JSON.stringify(params)
    };
    
    fetch(`/Manager/LockCandidateAddSoftLock`, requestOptions)
        .then(response => response.text())
        .then(response => {
            console.log(response);
            if (response == "Created") {
                //fetch(`/Manager/ManagerLandingPage`)
                //    /*.then(response => response.html)*/
                //    .then(data => {
                //        console.log(data);
                //    })
                //    .catch(error => { console.log(error) });
                alert('Successfully Created Request');
                window.location.replace("https://localhost:7283/Manager/ManagerLandingPage");
            }
            else {
                alert('oops..! Something is wrong.');
            }
        })
        .catch(error => { console.log(error) });
    $("#staticBackdrop").modal('hide');
    $('.modal-backdrop').remove();
}
let candidateID = null;
let employeeManager = null;
let requestMessage = null;
let empManagerStatus = null;

$(document).ready(function () {

    $("#reqCandidateListTbl").on('click', '#reqCandidateDetailsModal', showreqWaitingModal);
})

function showreqWaitingModal() {
    let currentRow = $(this).closest("tr");
    candidateID = currentRow.find(".employeeid").text();
    employeeManager = currentRow.find(".employeeManager").text();
    requestMessage = currentRow.find(".messageDes").text();

    $("#memberModal").modal('show');
    document.getElementById('showCandidateId').innerHTML = candidateID;
    document.getElementById('showManagerName').innerHTML = employeeManager; 
    document.getElementById('mgrMessage').innerHTML = requestMessage;

    $("#memberModalSaveBtn").click(function () {
        $("#memberModal").modal('hide');
        $('.modal-backdrop').remove();
        // call a post req to SoftLock Table        
        candidateLockConfirmReq();
    });
    $("#memberModalCancelBtn").click(function () {
        $("#showCandidateId").empty();
        $("#showManagerName").empty();
        $('#mgrMessage').val(null);
    });
}

async function candidateLockConfirmReq() {
    let selectElement = document.querySelector('#status');
    let output = selectElement.value;
    let Status = output;

    requestMessage = $('textarea[name="requestMsg"]').val();
    const params = {
        employeeId: candidateID.trim(),
        manager: employeeManager.trim(),
        requestMessage: requestMessage.trim(),
        Status: Status.trim(),
    }

    const requestOptions = {
        method: 'POST',
        headers: { 'Content-Type': 'application/json', "XSRF-TOKEN": $('input[name="__RequestVerificationToken"]').val() }, 
        body: JSON.stringify(params)
    };

    fetch(`/MemberMvc/CandidateReqProcess`, requestOptions)
        .then(response => response.text())
        .then(response => {
            console.log(response);
            if (response == "Created") {
                //fetch(`/MemberMvc/MemberLandingPage`)
                //    /*.then(response => response.html)*/
                //    .then(data => {
                //        console.log(data);
                //    })
                //    .catch(error => { console.log(error) });
                alert('Request Approved Successfully');
                window.location.replace("https://localhost:7283/MemberMvc/MemberLandingPage");
            }
            else {
                alert('Oops..! Something Is Wrong');
            }
        })
        .catch(error => { console.log(error) });
    $("#memberModal").modal('hide');
    $('.modal-backdrop').remove();
}
function FormateSSNNumber(Value) {
    var ValueClean = "";
    if (Value != undefined && Value != "" && Value != null) {
        Value = Value.replace(/[-  ]/g, '');
        if (Value.length == 9) {
            ValueClean = Value.replace(/(\d{3})\-?(\d{2})\-?(\d{4})/, "$1-$2-$3");
            $("#Employee_SSN").css({ "border": "1px solid #babec5" });
        }
        else if (Value.length > 9) {
            ValueClean = Value;
            $("#Employee_SSN").css({ "border": "1px solid red" });
        }
        else {
            ValueClean = Value;
            $("#Employee_SSN").css({ "border": "1px solid #babec5" });
        }
    }
    return ValueClean;
}
function FormatePhoneNumber(Value) {
    var ValueClean = "";
    if (Value != undefined && Value != "" && Value != null) {
        Value = Value.replace(/[-()  ]/g, '');
        if (Value.length == 10) {
            ValueClean = Value.replace(/(\d{3})\-?(\d{3})\-?(\d{4})/, "$1-$2-$3");
            $("#Employee_Phone").css({ "border": "1px solid #babec5" });
        }
        else if (Value.length > 10) {
            ValueClean = Value;
            $("#Employee_Phone").css({ "border": "1px solid red" });
        }
        else {
            $("#Employee_Phone").css({ "border": "1px solid red" });
            ValueClean = Value;
        }
    }
    return ValueClean;
}

var DeleteThisUserById = function (Id) {
    var url = domainurl + "/UserMgmt/DeleteUser";
    var param = JSON.stringify({
        id: Id
    });
    $.ajax({
        type: "POST",
        url: url,
        data: param,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        success: function (data) {
            if (data.result) {
                OpenSuccessMessageNew("Success!", data.message, function () {
                    LoadUserMgmt();
                });
            } else {
                OpenErrorMessageNew("Error!", data.message);
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {

        }
    });
}
function isEmail(email) {
    var regex = /^([a-zA-Z0-9_.+-])+\@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$/;
    if (regex.test(email)) {
        $("#Employee_Email").css("border-color", "#ccc");
    }
    else {
        $("#Employee_Email").css("border-color", "red");
    }
}
function isValidEmail(email) {
    console.log(email);
    var regex = /^([a-zA-Z0-9_.+-])+\@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$/;
    return regex.test(email);
}
var SaveThisUser = function () {
    console.log("Save User working");
    if (typeof ($("#UserPassword").val()) != "undefined" && $("#UserPassword").val() != null && $("#UserPassword").val() != "" && $("#UserPassword").val().length < 6) {
        OpenErrorMessageNew("Error!", "Password length should be 6 or more.");
    }
    else {
        var url = domainurl + "/UserMgmt/SaveUserInfo";
        var param = JSON.stringify({
            empid: empid,
            fnum: $("#Employee_FirstName").val(),
            lnum: $("#Employee_LastName").val(),
            email: $("#Employee_Email").val(),
            street: $("#Employee_Street").val(),
            StreetPrevious: $("#Employee_StreetPrevious").val(),
            city: $("#City").val(),
            state: $("#State").val(),
            zip: $("#ZipCode").val(),
            PasswordUpdateDays: $("#PasswordUpdateDays").val(),
            street2: $("#Street2").val(),
            city2: $("#CityPrevious").val(),
            state2: $("#StatePrevious").val(),
            zip2: $("#ZipCodePrevious").val(),
            phn: $("#Employee_Phone").val(),
            ssn: $("#Employee_SSN").val(),
            hire: $("#Employee_HireDate").val(),//DateHire.getDate(),
            place: $("#Employee_PlaceOfBirth").val(),
            job: $("#Employee_JobTitle").val(),
            session: $("#Employee_Session").val(),
            password: $("#UserConfirmPassword").val(),
            sales: $("#Employee_SalesCommissionStructure").val(),
            tech: $("#Employee_TechCommissionStructure").val(),
            username: $("#Employee_UserName").val(),
            iscalendar: $("#IsCalendar").prop('checked'),
            color: $("#CalendarColor").val(),
            NoAutoClockout: $("#Employee_NoAutoClockOut").is(":checked"),
            IsSupervisor: $("#Employee_IsSupervisor").is(":checked"),
            IsSalesMatrix: $("#Employee_IsSalesMatrix").is(":checked"),
            SuperVisorId: $("#Employee_SuperVisorId").val(),
            FireLicenseExpirationDate: $("#Employee_FireLicenseExpirationDate").val(),
            SalesLicenseExpirationDate: $("#Employee_SalesLicenseExpirationDate").val(),
            InstallLicenseExpirationDate: $("#Employee_InstallLicenseExpirationDate").val(),
            DriversLicenseExpirationDate: $("#Employee_DriversLicenseExpirationDate").val(),
            ClockInIp: $("#Employee_ClockInIP").val(),
            ispayroll: $("#ispayroll").prop('checked'),
            LicenseNo: $("#LicenseNo").val(),
            BranchId: $("#UserBranchDetails_BranchId").val(),
            BadgerUserId: $("#Employee_BadgerUserId").val(),
            employeetimeclocksupervisor: $("#TimeClockSupervisorId").val(),
            currentemp: $("#Employee_IsCurrentEmployee").prop('checked'),
            LeadSources: $("#LeadSources").val(),
            BrinksDelearUser: $("#Employee_BrinksDealerUser").val(),
            BrinksDelearPassword: $("#Employee_BrinksDealerPassword").val(),
            RouteList: encodeURI($("#Route").val())
        })
        $.ajax({
            type: "POST",
            url: url,
            data: param,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            cache: false,
            success: function (data) {
                if (data.result) {
                    OpenSuccessMessageNew("Success!", data.message, function () {
                        if (data.Logout) {
                            location.href = "/";
                        } else {
                            LoadUserInfo(data.userlogIntId, true);
                        }
                    });
                } else {
                    OpenErrorMessageNew("Error!", data.message, function () {

                    });
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                $(".loader-div").hide();
                console.log(errorThrown);
            }
        });
    }
    
}
var IsUserPasswordMatched = function () {
    if ($("#UserConfirmPassword").val() != $("#UserPassword").val()) {
        $("#errConfirmPass").removeClass('hidden');
        return false;
    } else {
        $("#errConfirmPass").addClass('hidden');
        return true;
    }
}
var ChangeUserPermission = function () {
    var url = domainurl + "/UserMgmt/UserPermissionChange";
    var param = JSON.stringify({
        empid: empid,
        permissionId: $("#PermissionGroup_Id").val()
    })
    $.ajax({
        type: "POST",
        url: url,
        data: param,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        success: function (data) {
            if (data.result) {
                OpenSuccessMessageNew("Success!", data.message, function () {

                });
            } else {
                OpenErrorMessageNew("Error!", data.message, function () {

                });
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            $(".loader-div").hide();
            console.log(errorThrown);
        }
    });
}
var IsUserExists = function () {
    var url = domainurl + "/UserMgmt/IsUserExistsGlobally/";
    var param = JSON.stringify({
        CurrentUsername: $("#CurrenUsername").val(),
        DesiredUsername: $("#Employee_UserName").val()
    });
    $.ajax({
        type: "POST",
        ajaxStart: $(".loader-div").show(),
        url: url,
        data: param,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        success: function (data) {
            if (data == false) {
                $(".userexistsmsg").addClass('hidden');
            } else {
                $(".userexistsmsg").removeClass('hidden');
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            $(".loader-div").hide();
            console.log(errorThrown);
        }
    });
}
var LoadHrTab = function () {
    $("#HrInfoTab").html(LoaderDom);
    $("#HrInfoTab").load(domainurl + "/Hr/HrHome/?UserId=" + userId);
    //$("#HrInfoTab").load(domainurl + "/Hr/HrHome");
  
}
var OpenVehicleMgmtTab = function () {
    $(".LoadVehicleInfo").load(domainurl + "/VehicleManagement/Index");
}
var LoadMgmtTab = function () {
    $(".LoadVehicleInfo").load(domainurl + "/Management/ManagementHome");
}
var LoadEmployeeReviewsTab = function () {
    console.log("hlww");
    $(".LoadEmployeeRevies").html(LoaderDom);
    $(".LoadEmployeeRevies").load(domainurl + "/Survey/EmployeeReviewsByUserId/?UserId=" + userId);
}
var HierarchyValidationCheck = function (UserId, SupervisorId) {
    console.log(UserId + " " + SupervisorId);
    $.ajax({
        type: "POST",
        ajaxStart: $(".loader-div").show(),
        url: domainurl + "/UserMgmt/HierarchyValidationCheck",
        data: JSON.stringify({
            Userid: UserId,
            SupervisorId: SupervisorId
        }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        success: function (data) {
            if (data) {
                $(".supervisor_hierarchy").addClass('hidden');
            }
            else {
                $(".supervisor_hierarchy").removeClass('hidden');
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            $(".loader-div").hide();
            console.log(errorThrown);
        }
    });
}
$(document).ready(function () {

    $("#UploadedPath").val(EmployeeProfilePicture);
    if ($("#UploadedPath").val() != "") {
        $(".chooseFilebtn").addClass("hidden");
        $(".changeFilebtn").removeClass("hidden");
        $(".deleteDoc").removeClass("hidden");
    }
    $("#UploadCustomerFileBtn").click(function () {
        $("#UploadedFile").click();
        $("#UploadSuccessMessage").addClass('hidden');
    });
    $(".change-picture-logo").click(function () {
        $("#UploadedFile").click();
        $("#UploadSuccessMessage").addClass('hidden');
    });
    $(".deleteDoc").click(function () {
        OpenConfirmationMessageNew("Confirm?", "Are you sure you want to delete this file?", function () {
            var delitem = selectedDeleteId;
            $.ajax({
                url: domainurl + "/UserMgmt/DeleteUserFile/?EmployeeId=" + empid,
                data: {
                    EmpleoyeeId: empid
                },
                type: "Post",
                dataType: "Json",
                success: function (result) {
                    if (result) {
                        $(".Upload_Doc").removeClass('hidden');
                        //$(".LoadPreviewDocument").addClass('hidden');
                        //$(".LoadPreviewDocument1").addClass('hidden');
                        $("#UploadCustomerFileBtn").attr('src', '/Content/img/Defaultimages.png');
                        $(".chooseFilebtn").removeClass("hidden");
                        $(".changeFilebtn").addClass("hidden");
                        $(".deleteDoc").addClass("hidden");
                        $("#Preview_Doc").attr('src', "");
                        $("#Frame_Doc").attr('src', "");
                        $("#UploadSuccessMessage").hide();
                        $("#description").val("");
                        $("#UploadedPath").val('');
                        $(".fileborder").addClass('border_none');
                        $("#UploadCustomerFileBtn").removeClass('otherfileposition');
                        $("#Value").val('');
                    }
                    else {
                        OpenErrorMessageNew("Error!", "Something Wrong", "");
                    }
                },

                error: function () {
                }
            });
        });
    })
    $('#UploadedFile').fileupload({
        pasteZone: null,
        url: domainurl + "/UserMgmt/SaveUserFile/?EmployeeId=" + empid,
        dataType: 'json',
        add: function (e, data) {
            if ($("#description").val() == "") {
                var filename = data.fileInput[0].value.split('\\').pop();
                $("#description").val(filename);
            }
            UserFileUploadjqXHRData = data;
        },
        progress: function (e, data) {
            var percentVal = parseInt(data.loaded / data.total * 100, 10);
            $(".file-progress").show();
            $(".file-progress .progress-bar").animate({
                width: percentVal + "%"
            }, 40);
            $(".file-progress .progress-bar span").text(percentVal + '%');
        },
        done: function (event, data) {
            setTimeout(function () {
                $(".file-progress").hide();
                $(".file-progress .progress-bar").animate({
                    width: 0 + "%"
                }, 0);
                $(".file-progress .progress-bar span").text(0 + '%');
            }, 500);
            console.log(data.result);
            if ((typeof (data.result.isUploaded) != "undefined") && data.result.isUploaded == true) {
                $("#UploadSuccessMessage").removeClass('hidden');
                //$("#UploadCustomerFileBtn").addClass('hidden');
                $("#UploadedPath").val(data.result.filePath);
                //var spfile = data.result.FullFilePath.split('.');
                $(".fileborder").removeClass('red-border');
                $("#uploadfileerror").addClass("hidden");
                $("#Value").val(data.result.filePath);
                $("#UploadCustomerFileBtn").attr('src', data.result.filePath)
                $(".chooseFilebtn").addClass("hidden");
                $(".changeFilebtn").removeClass("hidden");
                $(".deleteDoc").removeClass("hidden");
                $("#UploadCustomerFileBtn").addClass('custom-file');
                $("#UploadCustomerFileBtn").removeClass('otherfileposition');
                $(".fileborder").addClass('border_none');
            }
        },
        fail: function (event, data) {
            if (data.files[0].error) {
                alert(data.files[0].error);
            }
        }
    });
    $("#UploadedFile").on("change", function () {
        if (UserFileUploadjqXHRData) {
            UserFileUploadjqXHRData.submit();
        }
        return false;
    });
    DateHire = new Pikaday({
        format: 'MM/DD/YYYY',
        field: $('.AddHire')[0],
        trigger: $('#AddHireCustom')[0],
        firstDay: 1
    });
    FireExpDate = new Pikaday({
        format: 'MM/DD/YYYY',
        field: $('#Employee_FireLicenseExpirationDate')[0],
        trigger: $('#FireLicenseExpirationDateCustom')[0],
        firstDay: 1
    });
    SalesExpDate = new Pikaday({
        format: 'MM/DD/YYYY',
        field: $('#Employee_SalesLicenseExpirationDate')[0],
        trigger: $('#SalesLicenseExpirationDateCustom')[0],
        firstDay: 1
    });
    DriverExpDate = new Pikaday({
        format: 'MM/DD/YYYY',
        field: $('#Employee_DriversLicenseExpirationDate')[0],
        trigger: $('#DriversLicenseExpirationDateCustom')[0],
        firstDay: 1
    });
    InstallExpDate = new Pikaday({
        format: 'MM/DD/YYYY',
        field: $('#Employee_InstallLicenseExpirationDate')[0],
        trigger: $('#InstallLicenseExpirationDateCustom')[0],
        firstDay: 1
    });
    $('.selectpicker').selectpicker();

    $("#Employee_Email").keyup(function () {
        isEmail($("#Employee_Email").val());
    })
    parent.$('.close').click(function () {
        parent.$(".modal-body").html('');
    });
    $("#UploadProfilePic").click(function () {
        OpenRightToLeftModal(domainurl + "/UserMgmt/AddUserFile?id=" + empid);
    })
    $("#ChangeUserPermission").click(function () {
        OpenConfirmationMessageNew("Confirm?", "Are you sure you want to update this users permissions?", function () {
            ChangeUserPermission();
        });
    });
    $("#UserConfirmPassword").blur(function () {
        IsUserPasswordMatched();
    });
    $("#Employee_SSN").keyup(function () {
        var SSNNumber = $(this).val();
        if (SSNNumber != undefined && SSNNumber != null && SSNNumber != "") {
            var cleanSSNNumber = FormateSSNNumber(SSNNumber);
            $(this).val(cleanSSNNumber);
        }
    });
    $("#Employee_Phone").keyup(function () {
        var PhoneNumber = $(this).val();
        if (PhoneNumber != undefined && PhoneNumber != null && PhoneNumber != "") {
            var cleanPhoneNumber = FormatePhoneNumber(PhoneNumber);
            $(this).val(cleanPhoneNumber);
        }
    });
    $(".LoaderWorkingDiv").hide();
    $(".GoUser").click(function () {
        LoadUserMgmt();
    });
    $(".LoadDocInfo").load(domainurl + "/HrDoc/HrDocPartial?usernum=" + usernum);
    /*$("#edit-permissions").click(function () {
        OpenSuccessMessage("Attention", "If you edit permissions this user will be assigned to custom user, do you want to edit?", EditPermissions);
    });*/

    $("#DeleteThisUser").click(function () {

        var UserId = $("#DeleteThisUser").attr("dataid");
        OpenConfirmationMessageNew("Confirm?", "Are you sure you want to delete this user?", function () {
            DeleteThisUserById(UserId);
        });
    });
    /*$("#Employee_IsSupervisor").click(function (e, i) {
        if ($(e.target).is(":Checked")) {
            $(".supervisor_td").addClass("hidden");
            $("#Employee_SuperVisorId").val("-1");
        } else {
            $(".supervisor_td").removeClass("hidden");
        }
    });*/

    $("#btnSaveUser").click(function () {
        if (($("#Route").length > 0 && typeof (encodeURI($("#Route").val())) != "undefined") && (encodeURI($("#Route").val()) == "" || encodeURI($("#Route").val()) == "null" || encodeURI($("#Route").val()) == null)) {
            //$("#Route").css("border-color", "red");
            $(".Route").removeClass("hidden");
        }
        else {
            console.log("btnSaveUser clicked");
            if ($("#UserPassword").val() != "" && $("#UserConfirmPassword").val() != "") {
                if (IsUserPasswordMatched()) {
                    if ($("#Employee_Email").val() != "") {
                        if (isValidEmail($("#Employee_Email").val())) {
                            if (supervisorPermission == "True" && $(".supervisor_hierarchy").hasClass('hidden')) {
                                SaveThisUser();
                            }
                            else {
                                SaveThisUser();
                            }
                        }
                        else {
                            OpenErrorMessageNew("Error!", "Email address is not valid!", "");
                        }
                    }
                    else {
                        OpenErrorMessageNew("Error!", "Valid email address required!", "")
                    }
                }
            }
            else {
                if ($("#Employee_Email").val() != "") {
                    if (isValidEmail($("#Employee_Email").val())) {
                        if (supervisorPermission == "True" && $(".supervisor_hierarchy").hasClass('hidden')) {
                            SaveThisUser();
                        }
                        else {
                            SaveThisUser();
                        }
                    }
                    else {
                        OpenErrorMessageNew("Error!", "Email address is not valid!", "");
                    }
                }
                else {
                    OpenErrorMessageNew("Error!", "Valid email address required!", "")
                }
            }
        }
        
    });
    $('#TimeClockSupervisorId').on('change', function () {
        var thisObj = $(this);
        var isAllSelected = thisObj.find('option[value="00000000-0000-0000-0000-000000000000"]').prop('selected');
        var lastAllSelected = $(this).data('00000000-0000-0000-0000-000000000000');
        var selectedOptions = (thisObj.val()) ? thisObj.val() : [];
        var allOptionsLength = thisObj.find('option[value!="00000000-0000-0000-0000-000000000000"]').length;

        console.log(selectedOptions);
        var selectedOptionsLength = selectedOptions.length;

        if (isAllSelected == lastAllSelected) {

            if ($.inArray("00000000-0000-0000-0000-000000000000", selectedOptions) >= 0) {
                selectedOptionsLength -= 1;
            }

            if (allOptionsLength <= selectedOptionsLength) {

                thisObj.find('option[value="00000000-0000-0000-0000-000000000000"]').prop('selected', true).parent().selectpicker('refresh');
                isAllSelected = true;
            } else {
                thisObj.find('option[value="00000000-0000-0000-0000-000000000000"]').prop('selected', false).parent().selectpicker('refresh');
                isAllSelected = false;
            }

        } else {
            thisObj.find('option').prop('selected', isAllSelected).parent().selectpicker('refresh');
        }

        $(this).data('00000000-0000-0000-0000-000000000000', isAllSelected);
    }).trigger('change');
    $("#TimeClockSupervisorId").selectpicker('val', timeclocksupervisor);
    $("#Employee_SuperVisorId").change(function () {
        var SupervisorId = $(this).val();
        if (typeof (SupervisorId) != "undefined" && SupervisorId != "-1") {
            HierarchyValidationCheck(userId, SupervisorId);
        }
        else {
            $(".supervisor_hierarchy").addClass('hidden');
        }
    })
});
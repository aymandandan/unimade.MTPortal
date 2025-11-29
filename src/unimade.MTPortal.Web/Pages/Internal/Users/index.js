$(function () {
    var l = abp.localization.getResource('MTPortal');
    var li = abp.localization.getResource('AbpIdentity');

    var createModal = new abp.ModalManager(abp.appPath + 'Internal/Users/CreateModal');
    var editModal = new abp.ModalManager(abp.appPath + 'Internal/Users/EditModal');

    var dataTable = $('#PublicUsersTable').DataTable(
        abp.libs.datatables.normalizeConfiguration({
            serverSide: true,
            paging: true,
            order: [[1, 'desc']],
            searching: true,
            scrollX: true,
            ajax: abp.libs.datatables.createAjax(unimade.mTPortal.identity.extendedIdentityUser.getPublicUsersList),
            columnDefs: [
                {
                    title: l('Actions'),
                    rowAction: {
                        items:
                            [
                                {
                                    text: l('Edit'),
                                    action: function (data) {
                                        editModal.open({ id: data.record.id });
                                    }
                                },
                                {
                                    text: l('Delete'),
                                    confirmMessage: function (data) {
                                        return l('PublicUsertDeletionConfirmationMessage', data.record.userName);
                                    },
                                    action: function (data) {
                                        unimade.mTPortal.identity.extendedIdentityUser
                                            .delete(data.record.id)
                                            .then(function () {
                                                abp.notify.info(l('SuccessfullyDeleted'));
                                                dataTable.ajax.reload();
                                            });
                                    }
                                }
                            ]
                    }
                },
                {
                    title: li('UserName'),
                    data: "userName"
                },
                {
                    title: li('Email'),
                    data: "email"
                },
                {
                    title: li('PhoneNumber'),
                    data: 'phoneNumber',
                },
                {
                    title: l('CreationTime'),
                    data: "creationTime",
                    render: function (data) {
                        return luxon
                            .DateTime
                            .fromISO(data, {
                                locale: abp.localization.currentCulture.name,
                            }).toLocaleString();
                    }
                }
            ]
        })
    );

    createModal.onResult(function () {
        dataTable.ajax.reload();
    });

    editModal.onResult(function () {
        dataTable.ajax.reload();
    })

    $('#NewPublicUserButton').on("click", function (e) {
        e.preventDefault();
        createModal.open();
    })
})
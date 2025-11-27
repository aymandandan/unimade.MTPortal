$(function () {
    var l = abp.localization.getResource('MTPortal');

    var createModal = new abp.ModalManager(abp.appPath + 'Internal/Announcements/CreateModal');
    var editModal = new abp.ModalManager(abp.appPath + 'Internal/Announcements/EditModal');

    var dataTable = $('#AnnouncementsTable').DataTable(
        abp.libs.datatables.normalizeConfiguration({
            serverSide: true,
            paging: true,
            order: [[1, 'desc']],
            searching: true,
            scrollX: true,
            ajax: abp.libs.datatables.createAjax(unimade.mTPortal.announcements.announcement.getList),
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
                                    return l('AnnouncementDeletionConfirmationMessage', data.record.title);
                                },
                                action: function (data) {
                                    unimade.mTPortal.announcements.announcement
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
                    title: l('Title'),
                    data: "title"
                },
                {
                    title: l('Content'),
                    data: "content",
                    render: function (data) {
                        return data.length > 50 ? data.substr(0, 50) + '...' : data;
                    }
                },
                {
                    title: l('IsPublished'),
                    data: "isPublished",
                    render: function (data) {
                        return data ? l('Yes') : l('No');
                    }
                },
                {
                    title: l('PublishDate'),
                    data: "publishDate",
                    render: function (data) {
                        return luxon
                            .DateTime
                            .fromISO(data, {
                                locale: abp.localization.currentCulture.name,
                            }).toLocaleString();
                    }
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

    $('#NewAnnouncementButton').on("click", function (e) {
        e.preventDefault();
        createModal.open();
    })
})
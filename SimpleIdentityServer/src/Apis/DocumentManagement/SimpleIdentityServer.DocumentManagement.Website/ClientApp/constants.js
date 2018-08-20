module.exports = {
    clientId: 'DocumentManagementWebsite',
    events: {
        USER_LOGGED_IN: 'USER_LOGGED_IN',
        USER_LOGGED_OUT: 'USER_LOGGED_OUT',
        SESSION_CREATED: 'SESSION_CREATED',
        SESSION_UPDATED: 'SESSION_UPDATED',
        DISPLAY_MESSAGE: 'DISPLAY_MESSAGE'
    },
    adminuiUrl: 'http://localhost:64951',
    openidUrl: 'http://localhost:60000',
    openidManagerBaseUrl: 'http://localhost:60003',
    profileBaseUrl: 'http://localhost:60005',
    hierarchicalResourcesBaseUrl: 'http://localhost:60006',
    eventSourceUrl: 'http://localhost:60002',
};
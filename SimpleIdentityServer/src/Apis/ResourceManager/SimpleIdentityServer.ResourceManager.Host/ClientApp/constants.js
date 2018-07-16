module.exports = {
    clientId: 'ResourceManagerClientId',
    events: {
        USER_LOGGED_IN: 'USER_LOGGED_IN',
        USER_LOGGED_OUT: 'USER_LOGGED_OUT',
        SESSION_CREATED: 'SESSION_CREATED',
        SESSION_UPDATED: 'SESSION_UPDATED',
        DISPLAY_MESSAGE: 'DISPLAY_MESSAGE'
    },
    openidManagerBaseUrl: 'http://localhost:60006/openidmanager',
    profileBaseUrl: 'http://localhost:60006/profile',
    hierarchicalResourcesBaseUrl: 'http://localhost:60006/resources',
    eventSourceUrl: 'http://localhost:60002'
};
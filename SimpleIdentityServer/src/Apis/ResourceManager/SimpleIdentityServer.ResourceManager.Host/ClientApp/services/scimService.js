import $ from 'jquery';
import Constants from '../constants';
import SessionService from './sessionService';
import { SessionStore } from '../stores';

module.exports = {
    /**
    * Get the schemas.
    */
    getSchemas: function() {
        return new Promise(function (resolve, reject) {
            var session = SessionService.getSession();
            $.ajax({
                url: SessionStore.getSession().selectedScim.url + '/schemas',
                method: "GET",
                headers: {
                    "Authorization": "Bearer "+ session.token
                }
            }).then(function (data) {
                resolve(data);
            }).fail(function (e) {
                reject(e);
            });
        });        
    },
    /**
    * Get the schemas.
    */
    getSchema: function (schemaId) {
        return new Promise(function (resolve, reject) {
            var session = SessionService.getSession();
            $.ajax({
                url: SessionStore.getSession().selectedScim.url + '/schemas/' + schemaId,
                method: "GET",
                headers: {
                    "Authorization": "Bearer " + session.token
                }
            }).then(function (data) {
                resolve(data);
            }).fail(function (e) {
                reject(e);
            });
        });
    },
    /**
    * Search the users.
    */
    searchUsers: function(request) {        
        return new Promise(function (resolve, reject) {
            var data = JSON.stringify(request);
            var session = SessionService.getSession();
            $.ajax({
                url: SessionStore.getSession().selectedScim.url + '/users/.search',
                method: "POST",
                data: data,
                contentType: 'application/json',
                headers: {
                    "Authorization": "Bearer "+ session.token
                }
            }).then(function (data) {
                resolve(data);
            }).fail(function (e) {
                reject(e);
            });
        });
    },
    /**
    * Get the user.
    */
    getUser: function(resourceId) {
        return new Promise(function (resolve, reject) {
            var session = SessionService.getSession();
            $.ajax({
                url: SessionStore.getSession().selectedScim.url + '/users/' + resourceId,
                method: "GET",
                headers: {
                    "Authorization": "Bearer "+ session.token
                }
            }).then(function (data) {
                resolve(data);
            }).fail(function (e) {
                reject(e);
            });
        });        
    },
    /**
    * Get the users.
    */
    getUsers: function() {
        return new Promise(function (resolve, reject) {
            var session = SessionService.getSession();
            $.ajax({
                url: SessionStore.getSession().selectedScim.url + '/users',
                method: "GET",
                headers: {
                    "Authorization": "Bearer "+ session.token
                }
            }).then(function (data) {
                resolve(data);
            }).fail(function (e) {
                reject(e);
            });
        });        
    },
    /**
    * Search the groups.
    */
    searchGroups: function(request) {        
        return new Promise(function (resolve, reject) {
            var data = JSON.stringify(request);
            var session = SessionService.getSession();
            $.ajax({
                url: SessionStore.getSession().selectedScim.url + '/groups/.search',
                method: "POST",
                data: data,
                contentType: 'application/json',
                headers: {
                    "Authorization": "Bearer "+ session.token
                }
            }).then(function (data) {
                resolve(data);
            }).fail(function (e) {
                reject(e);
            });
        });
    },
    /**
    * Get the group.
    */
    getGroup: function(resourceId) {
        return new Promise(function (resolve, reject) {
            var session = SessionService.getSession();
            $.ajax({
                url: SessionStore.getSession().selectedScim.url + '/groups/' + resourceId,
                method: "GET",
                headers: {
                    "Authorization": "Bearer "+ session.token
                }
            }).then(function (data) {
                resolve(data);
            }).fail(function (e) {
                reject(e);
            });
        });        
    },
    /**
    * Get all the mappings.
    */
    getAllMappings: function () {
        return new Promise(function (resolve, reject) {
            var session = SessionService.getSession();
            $.ajax({
                url: SessionStore.getSession().selectedScim.url + '/mappings',
                method: "GET",
                headers: {
                    "Authorization": "Bearer " + session.token
                }
            }).then(function (data) {
                resolve(data);
            }).fail(function (e) {
                reject(e);
            });
        });
    },
    /**
    * Get all the AD properties
    */
    getAllAdProperties: function (schemaId) {
        return new Promise(function (resolve, reject) {
            console.log(SessionStore.getSession());
            var session = SessionService.getSession();
            $.ajax({
                url: SessionStore.getSession().selectedScim.url + '/mappings/properties/' + schemaId,
                method: "GET",
                headers: {
                    "Authorization": "Bearer " + session.token
                }
            }).then(function (data) {
                resolve(data);
            }).fail(function (e) {
                reject(e);
            });
        });
    },
    /**
    * Add scim mapping
    */
    addScimMapping: function (request) {
        return new Promise(function (resolve, reject) {
            var session = SessionService.getSession();
            $.ajax({
                url: SessionStore.getSession().selectedScim.url + '/mappings',
                method: "POST",
                data: JSON.stringify(request),
                contentType: 'application/json',
                headers: {
                    "Authorization": "Bearer " + session.token
                }
            }).then(function (data) {
                resolve(data);
            }).fail(function (e) {
                reject(e);
            });
        });
    },
    /**
    * Remove scim mapping.
    */
    removeScimMapping: function (attributeId) {
        return new Promise(function (resolve, reject) {
            var session = SessionService.getSession();
            $.ajax({
                url: SessionStore.getSession().selectedScim.url + '/mappings/' + attributeId,
                method: "DELETE",
                headers: {
                    "Authorization": "Bearer " + session.token
                }
            }).then(function (data) {
                resolve(data);
            }).fail(function (e) {
                reject(e);
            });
        });
    },
    /**
     * Get AD configuration.
     */
    getAdConfiguration: function () {
        return new Promise(function (resolve, reject) {
            var session = SessionService.getSession();
            $.ajax({
                url: SessionStore.getSession().selectedScim.url + '/adconfiguration',
                method: "GET",
                headers: {
                    "Authorization": "Bearer " + session.token
                }
            }).then(function (data) {
                resolve(data);
            }).fail(function (e) {
                reject(e);
            });
        });
    },
    /**
    * Update AD configuration
    */
    updateAdConfiguration: function(request) {
        return new Promise(function (resolve, reject) {
            var session = SessionService.getSession();
            $.ajax({
                url: SessionStore.getSession().selectedScim.url + '/adconfiguration',
                method: "PUT",
                data: JSON.stringify(request),
                contentType: 'application/json',
                headers: {
                    "Authorization": "Bearer " + session.token
                }
            }).then(function (data) {
                resolve(data);
            }).fail(function (e) {
                reject(e);
            });
        });
    }
};
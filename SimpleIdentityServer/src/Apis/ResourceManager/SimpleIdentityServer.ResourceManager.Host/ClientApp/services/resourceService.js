import $ from 'jquery';
import Constants from '../constants';
import SessionService from './sessionService';
import { SessionStore } from '../stores';

module.exports = {
	/**
	* Search the resources.
	*/
	search: function(request) {
        return new Promise(function (resolve, reject) {
            var data = JSON.stringify(request);
            var session = SessionService.getSession();
            var url = SessionStore.getSession().selectedAuth['url'];
            $.get(url).then(function(configuration) {
                $.ajax({
                    url: configuration['resource_registration_endpoint'] + '/.search',
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
            }).fail(function(e) {
                reject(e);
            });
		});
	},
    /**
    * Get the authorization policies.
    */
    getAuthPolicies: function(id) {
        return new Promise(function (resolve, reject) {
            var session = SessionService.getSession();
            var url = SessionStore.getSession().selectedAuth['url'];
            $.get(url).then(function(configuration) {
                var data = JSON.stringify({ resource_ids : [ id ], count: 10000 });
                $.ajax({
                    url: configuration['policies_endpoint'] + '/.search',
                    method: "POST",
                    data: data,
                    contentType: 'application/json',
                    headers: {
                        "Authorization": "Bearer "+ session.token
                    }
                }).then(function (r) {
                    resolve(r);
                }).fail(function (e) {
                    reject(e);
                });
            });
        });        
    },
    /**
    * Get the resource.
    */
    get: function(id) {
        return new Promise(function (resolve, reject) {
            var session = SessionService.getSession();
            var url = SessionStore.getSession().selectedAuth['url'];
            $.get(url).then(function(configuration) {
                $.ajax({
                    url: configuration['resource_registration_endpoint'] + '/' + id,
                    method: "GET",
                    headers: {
                        "Authorization": "Bearer "+ session.token
                    }
                }).then(function (data) {
                    resolve(data);
                }).fail(function (e) {
                    reject(e);
                });
            }).fail(function(e) {
                reject(e);
            });
            
        });        
    },
    /**
    * Get the resource.
    */
    getAll: function(id) {
        return new Promise(function (resolve, reject) {
            var session = SessionService.getSession();
            var url = SessionStore.getSession().selectedAuth['url'];
            $.get(url).then(function(configuration) {
                $.ajax({
                    url: configuration['resource_registration_endpoint'],
                    method: "GET",
                    headers: {
                        "Authorization": "Bearer "+ session.token
                    }
                }).then(function (data) {
                    resolve(data);
                }).fail(function (e) {
                    reject(e);
                });
            }).fail(function(e) {
                reject(e);
            });
            
        });        
    },
    /**
    * Remove the resource.
    */
    delete: function(id) {
        return new Promise(function (resolve, reject) {
            var session = SessionService.getSession();
            var url = SessionStore.getSession().selectedAuth['url'];
            $.get(url).then(function(configuration) {
                $.ajax({
                    url: configuration['resource_registration_endpoint'] + '/' + id,
                    method: "DELETE",
                    headers: {
                        "Authorization": "Bearer "+ session.token
                    }
                }).then(function (data) {
                    resolve(data);
                }).fail(function (e) {
                    reject(e);
                });
            }).fail(function(e) {
                reject(e);
            });            
        });
    },
    /**
    * Add the resource.
    */
    add: function(request) {
        return new Promise(function (resolve, reject) {
            var data = JSON.stringify(request);
            var session = SessionService.getSession();
            var url = SessionStore.getSession().selectedAuth['url'];
            $.get(url).then(function(configuration) {
                $.ajax({
                    url: configuration['resource_registration_endpoint'],
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
            }).fail(function(e) {
                reject(e);
            });
        });
    },
    /**
    * Update the resource.
    */
    update: function(request) {
        return new Promise(function (resolve, reject) {
            var data = JSON.stringify(request);
            var session = SessionService.getSession();
            var url = SessionStore.getSession().selectedAuth['url'];
            $.get(url).then(function(configuration) {
                $.ajax({
                    url: configuration['resource_registration_endpoint'],
                    method: "PUT",
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
            }).fail(function(e) {
                reject(e);
            });
        });
    }
};
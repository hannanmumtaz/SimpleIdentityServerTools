import $ from 'jquery';
import Constants from '../constants';
import SessionService from './sessionService';
import { SessionStore } from '../stores';

module.exports = {
	/**
	* Search the resource owners.
	*/
	search: function(request, type) {
        return new Promise(function (resolve, reject) {
            var data = JSON.stringify(request);
            var session = SessionService.getSession();
            $.get(SessionStore.getSession().selectedOpenid['manager_url']).then(function(configuration) {                
                $.ajax({
                    url: configuration['resourceowners_endpoint'] + '/.search',
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
	* Get the resource owner.
	*/
	get: function(id) {
		return new Promise(function(resolve, reject) {
            var session = SessionService.getSession();
            $.get(SessionStore.getSession().selectedOpenid['manager_url']).then(function(configuration) { 
                $.ajax({
                    url: configuration['resourceowners_endpoint'] + '/' + id,
                    method: "GET",
                    headers: {
                        "Authorization": "Bearer "+ session.token
                    }
                }).then(function(data) {
                    resolve(data);
                }).fail(function(e) {
                    reject(e);
                });                
            }).fail(function(e) {
                reject(e);
            });
		});
	},
    /**
    * Get the resource owner.
    */
    getAll: function(id) {
        return new Promise(function(resolve, reject) {
            var session = SessionService.getSession();
            $.get(SessionStore.getSession().selectedOpenid['manager_url']).then(function(configuration) { 
                $.ajax({
                    url: configuration['resourceowners_endpoint'],
                    method: "GET",
                    headers: {
                        "Authorization": "Bearer "+ session.token
                    }
                }).then(function(data) {
                    resolve(data);
                }).fail(function(e) {
                    reject(e);
                });                
            }).fail(function(e) {
                reject(e);
            });
        });
    },
	/**
	* Remove the user.
	*/
	delete: function(id) {
		return new Promise(function(resolve, reject) {
            var session = SessionService.getSession();
            $.get(SessionStore.getSession().selectedOpenid['manager_url']).then(function(configuration) { 
                $.ajax({
                    url: configuration['resourceowners_endpoint'] + '/' + id,
                    method: "DELETE",
                    headers: {
                        "Authorization": "Bearer "+ session.token
                    }
                }).then(function(data) {
                    resolve(data);
                }).fail(function(e) {
                    reject(e);
                });             
            }).fail(function(e) {
                reject(e);
            });
		});
	},
    /**
    * Add a resource owner.
    */
    add: function(request) {
        return new Promise(function (resolve, reject) {
            var data = JSON.stringify(request);
            var session = SessionService.getSession();
            $.get(SessionStore.getSession().selectedOpenid['manager_url']).then(function(configuration) {                
                $.ajax({
                    url: configuration['resourceowners_endpoint'],
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
    * Update the claims.
    */
    updateClaims: function(request) {
        return new Promise(function (resolve, reject) {
            var data = JSON.stringify(request);
            var session = SessionService.getSession();
            $.get(SessionStore.getSession().selectedOpenid['manager_url']).then(function(configuration) { 
                $.ajax({
                    url: configuration['resourceowners_endpoint'] + '/claims',
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
    },
    /**
    * Update password.
    */
    updatePassword: function(request) {
        return new Promise(function (resolve, reject) {
            var data = JSON.stringify(request);
            var session = SessionService.getSession();
            $.get(SessionStore.getSession().selectedOpenid['manager_url']).then(function(configuration) { 
                $.ajax({
                    url: configuration['resourceowners_endpoint'] + '/password',
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
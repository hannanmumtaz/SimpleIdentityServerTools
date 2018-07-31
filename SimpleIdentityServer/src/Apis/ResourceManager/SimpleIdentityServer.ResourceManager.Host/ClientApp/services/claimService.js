import $ from 'jquery';
import Constants from '../constants';
import SessionService from './sessionService';
import { SessionStore } from '../stores';

module.exports = {
	/**
	* Search the claims.
	*/
	search: function(request) {
        return new Promise(function (resolve, reject) {
            var data = JSON.stringify(request);
            var session = SessionService.getSession();
            $.get(SessionStore.getSession().selectedOpenid['manager_url']).then(function(configuration) {
                $.ajax({
                    url: configuration['claims_endpoint'] + '/.search',
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
	* Get the claim.
	*/
	get: function(id) {
		return new Promise(function(resolve, reject) {
            var session = SessionService.getSession();
            $.get(SessionStore.getSession().selectedOpenid['manager_url']).then(function(configuration) {
                $.ajax({
                    url: configuration['claims_endpoint'] + '/' +id,
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
    * Get all the claims.
    */
    getAll: function() {
        return new Promise(function(resolve, reject) {
            var session = SessionService.getSession();
            $.get(SessionStore.getSession().selectedOpenid['manager_url']).then(function(configuration) {
                $.ajax({
                    url: configuration['claims_endpoint'],
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
    * Add a claim.
    */
    add: function(request) {
        return new Promise(function (resolve, reject) {
            var data = JSON.stringify(request);
            var session = SessionService.getSession();
            $.get(SessionStore.getSession().selectedOpenid['manager_url']).then(function(configuration) {
                $.ajax({
                    url: configuration['claims_endpoint'],
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
    * Remove a scope.
    */
    delete: function(id) {
        return new Promise(function (resolve, reject) {
            var session = SessionService.getSession();
            $.get(SessionStore.getSession().selectedOpenid['manager_url']).then(function(configuration) {
                $.ajax({
                    url: configuration['claims_endpoint'] + '/' + id,
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
    }
};
import $ from 'jquery';
import Constants from '../constants';
import SessionService from './sessionService';
import { SessionStore } from '../stores';

module.exports = {
	/**
	* Search the clients.
	*/
	search: function(request, type) {
        return new Promise(function (resolve, reject) {
            var data = JSON.stringify(request);
            var session = SessionService.getSession();
            var url = type === 'openid' ? SessionStore.getSession().selectedOpenid['manager_url'] : SessionStore.getSession().selectedAuth['manager_url'];
            $.get(url).then(function(configuration) {
                $.ajax({
                    url: configuration['clients_endpoint'] + '/.search',
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
	* Get the client.
	*/
	get: function(id, type) {
		return new Promise(function(resolve, reject) {
            var session = SessionService.getSession();
            var url = type === 'openid' ? SessionStore.getSession().selectedOpenid['manager_url'] : SessionStore.getSession().selectedAuth['manager_url'];
            $.get(url).then(function(configuration) {
                $.ajax({
                    url: configuration['clients_endpoint'] + '/' +id,
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
    * Get all the clients
    */
    getAll: function(type) {
        return new Promise(function(resolve, reject) {
            var session = SessionService.getSession();
            var url = type === 'openid' ? SessionStore.getSession().selectedOpenid['manager_url'] : SessionStore.getSession().selectedAuth['manager_url'];
            $.get(url).then(function(configuration) {
                $.ajax({
                    url: configuration['clients_endpoint'],
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
    * Get the client.
    */
    remove: function(id, type) {
        return new Promise(function(resolve, reject) {
            var session = SessionService.getSession();
            var url = type === 'openid' ? SessionStore.getSession().selectedOpenid['manager_url'] : SessionStore.getSession().selectedAuth['manager_url'];
            $.get(url).then(function(configuration) {
                $.ajax({
                    url: configuration['clients_endpoint'] + '/' +id,
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
    * Add a client.
    */
    add: function(request, type) {
        return new Promise(function (resolve, reject) {
            var data = JSON.stringify(request);
            var session = SessionService.getSession();
            var url = type === 'openid' ? SessionStore.getSession().selectedOpenid['manager_url'] : SessionStore.getSession().selectedAuth['manager_url'];
            $.get(url).then(function(configuration) {
                $.ajax({
                    url: configuration['clients_endpoint'],
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
    * Update a client.
    */
    update: function(request, type) {
        return new Promise(function (resolve, reject) {
            var data = JSON.stringify(request);
            var session = SessionService.getSession();
            var url = type === 'openid' ? SessionStore.getSession().selectedOpenid['manager_url'] : SessionStore.getSession().selectedAuth['manager_url'];
            $.get(url).then(function(configuration) {
                $.ajax({
                    url: configuration['clients_endpoint'],
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
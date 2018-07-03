import $ from 'jquery';
import Constants from '../constants';
import SessionService from './sessionService';
import { SessionStore } from '../stores';

module.exports = {
	/**
	* Update the authorization policy.
	*/
	update: function(request) {
        return new Promise(function (resolve, reject) {
            var data = JSON.stringify(request);
            var session = SessionService.getSession();
            var url = SessionStore.getSession().selectedAuth['url'];
            $.get(url).then(function(configuration) {
                $.ajax({
                    url: configuration['policies_endpoint'],
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
    * Add the authorization policy.
    */
    add: function(request) {
        return new Promise(function (resolve, reject) {
            var data = JSON.stringify(request);
            var session = SessionService.getSession();
            var url = SessionStore.getSession().selectedAuth['url'];
            console.log(url);
            $.get(url).then(function(configuration) {
                $.ajax({
                    url: configuration['policies_endpoint'],
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
    * Remove the resource.
    */
    delete: function(id) {
        return new Promise(function (resolve, reject) {
            var session = SessionService.getSession();
            var url = SessionStore.getSession().selectedAuth['url'];
            $.get(url).then(function(configuration) {
                $.ajax({
                    url: configuration['policies_endpoint'] + '/' + id,
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
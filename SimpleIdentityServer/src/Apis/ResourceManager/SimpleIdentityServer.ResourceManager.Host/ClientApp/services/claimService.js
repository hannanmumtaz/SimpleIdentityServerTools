import $ from 'jquery';
import Constants from '../constants';
import SessionService from './sessionService';

module.exports = {
	/**
	* Search the claims.
	*/
	search: function(request) {
        return new Promise(function (resolve, reject) {
            var data = JSON.stringify(request);
            var session = SessionService.getSession();
            $.ajax({
                url: Constants.openidManagerBaseUrl + '/api/claims/.search',
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
	* Get the claim.
	*/
	get: function(id) {
		return new Promise(function(resolve, reject) {
            var session = SessionService.getSession();
			$.ajax({
				url: Constants.openidManagerBaseUrl + '/api/claims/' +id,
                method: "GET",
                headers: {
                	"Authorization": "Bearer "+ session.token
                }
			}).then(function(data) {
				resolve(data);
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
            $.ajax({
                url: Constants.openidManagerBaseUrl + '/api/claims',
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
    * Remove a scope.
    */
    delete: function(id) {
        return new Promise(function (resolve, reject) {
            var session = SessionService.getSession();
            $.ajax({
                url: Constants.openidManagerBaseUrl + '/api/claims/' + id,
                method: "DELETE",
                headers: {
                    "Authorization": "Bearer "+ session.token
                }
            }).then(function (data) {
                resolve(data);
            }).fail(function (e) {
                reject(e);
            });
        });
    }
};
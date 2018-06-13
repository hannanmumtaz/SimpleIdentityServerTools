import $ from 'jquery';
import Constants from '../constants';
import SessionService from './sessionService';

module.exports = {
	/**
	* Get the modules.
	*/
	getModules: function(type) {
        return new Promise(function (resolve, reject) {
            var session = SessionService.getSession();
            $.ajax({
                url: Constants.apiUrl + '/parameters/' + type,
                method: "GET",
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
	* Update the parameters.
	*/
	updateParameters: function(request, type) {
		return new Promise(function(resolve, reject) {
            var data = JSON.stringify(request);
            var session = SessionService.getSession();
			$.ajax({
                url: Constants.apiUrl + '/parameters/' + type,
                method: "PUT",
                data: data,
                contentType: 'application/json',
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
    * Get the connectors.
    */
    getConnectors: function() {
        return new Promise(function (resolve, reject) {
            var session = SessionService.getSession();
            $.ajax({
                url: Constants.apiUrl + '/parameters/connectors',
                method: "GET",
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
    * Update the connectors.
    */
    updateConnectors: function(request) {
        return new Promise(function (resolve, reject) {
            var data = JSON.stringify(request);
            var session = SessionService.getSession();
            $.ajax({
                url: Constants.apiUrl + '/parameters/connectors',
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
        });
    }
};
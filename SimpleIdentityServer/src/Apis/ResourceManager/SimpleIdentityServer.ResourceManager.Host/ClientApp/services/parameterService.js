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
                headers: {
                	"Authorization": "Bearer "+ session.token
                }
			}).then(function(data) {
				resolve(data);
			}).fail(function(e) {
				reject(e);
			});
		});
	}
};
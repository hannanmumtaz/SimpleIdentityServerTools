import $ from 'jquery';
import Constants from '../constants';
import SessionService from './sessionService';

module.exports = {
	/**
	* Get all the user profiles.
	*/
	getUserProfiles: function(subject) {
        return new Promise(function (resolve, reject) {
            var session = SessionService.getSession();
            $.ajax({
                url: "http://localhost:60000/profiles/" + subject, // TODO : EXTERNALIZE THIS URL.
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
	* Link a user profile.
	*/
	linkUserProfile: function(subject, request) {
        return new Promise(function (resolve, reject) {
            var session = SessionService.getSession();
            var data = JSON.stringify(request);
            $.ajax({
                url: "http://localhost:60000/profiles/" + subject, // TODO : EXTERNALIZE THIS URL.
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
    * Get the user profile.
    */
    unlinkUserProfile: function(subject, externalSubject) {
        return new Promise(function (resolve, reject) {
            var session = SessionService.getSession();
            $.ajax({
                url: "http://localhost:60000/profiles/" + subject + '/' + externalSubject, // TODO : EXTERNALIZE THIS URL.
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
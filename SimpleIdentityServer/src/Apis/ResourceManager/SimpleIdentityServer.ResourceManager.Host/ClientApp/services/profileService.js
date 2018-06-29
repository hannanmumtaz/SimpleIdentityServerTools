import $ from 'jquery';
import Constants from '../constants';
import SessionService from './sessionService';

module.exports = {
    /**
    * Update mine profile.
    */
    updateMineProfile: function(request) {
        return new Promise(function (resolve, reject) {
            var data = JSON.stringify(request);
            var session = SessionService.getSession();
            $.ajax({
                url: Constants.profileBaseUrl + '/profiles/.me',
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
    },
    /**
    * Get mine profile.
    */
    getMineProfile: function(id) {
        return new Promise(function (resolve, reject) {
            var session = SessionService.getSession();
            $.ajax({
                url: Constants.profileBaseUrl + '/profiles/.me',
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
    }
};
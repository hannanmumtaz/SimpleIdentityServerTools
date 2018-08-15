import $ from 'jquery';
import Constants from '../constants';
import SessionService from './sessionService';

module.exports = {
    /**
    * Get all the user profiles.
    */
    getAllFilters: function(subject) {
        return new Promise(function (resolve, reject) {
            var session = SessionService.getSession();
            $.ajax({
                url: "http://localhost:60000/filters", // TODO : EXTERNALIZE THIS URL.
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
    * Add an account filter
    */
    addAccountFilter: function(request) {
        return new Promise(function (resolve, reject) {
            var session = SessionService.getSession();
            var data = JSON.stringify(request);
            $.ajax({
                url: "http://localhost:60000/filters", // TODO : EXTERNALIZE THIS URL.
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
    * Remove the account filter.
    */
    deleteAccountFilter: function(filterId) {
        return new Promise(function (resolve, reject) {
            var session = SessionService.getSession();
            $.ajax({
                url: "http://localhost:60000/filters/" + filterId, // TODO : EXTERNALIZE THIS URL.
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
    },
    /**
    * Get the account filter.
    */
    getAccountFilter: function(filterId) {
        return new Promise(function (resolve, reject) {
            var session = SessionService.getSession();
            $.ajax({
                url: "http://localhost:60000/filters/" + filterId, // TODO : EXTERNALIZE THIS URL.
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
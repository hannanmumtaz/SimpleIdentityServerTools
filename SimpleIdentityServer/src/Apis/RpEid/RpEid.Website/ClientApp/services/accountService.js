import $ from 'jquery';
import Constants from '../constants';
import SessionService from './sessionService';

module.exports = {
    /**
    * Search the accounts.
    */
    search: function (request) {
        return new Promise(function (resolve, reject) {
            var data = JSON.stringify(request);
            var session = SessionService.getSession();
            $.ajax(Constants.apiUrl + '/accounts/.search', {
                type: 'POST',
                contentType: 'application/json',
                data: data,
                headers: {
                    "Authorization": "Bearer " + session.token
                }
            }).then(function (data) {
                resolve(data);
            }).fail(function (e) {
                reject(e);
            });
        });
    },
    /**
    * Get the account
    */
    getMine: function () {
        return new Promise(function (resolve, reject) {
            var session = SessionService.getSession();
            $.ajax(Constants.apiUrl + '/accounts/.me', {
                type: 'GET',
                headers: {
                    "Authorization": "Bearer " + session.token
                }
            }).then(function (data) {
                resolve(data);
            }).fail(function (e) {
                reject(e);
            });
        });
    },
    /**
    * Grant access.
    */
    grant: function (id) {
        return new Promise(function (resolve, reject) {
            var session = SessionService.getSession();
            $.ajax(Constants.apiUrl + '/accounts/.grant/' + id, {
                type: 'GET',
                headers: {
                    "Authorization": "Bearer " + session.token
                }
            }).then(function (data) {
                resolve(data);
            }).fail(function (e) {
                reject(e);
            });
        });
    },
    /**
    * Confirm access.
    */
    confirm: function (id) {
        return new Promise(function (resolve, reject) {
            var session = SessionService.getSession();
            $.ajax(Constants.apiUrl + '/accounts/.confirm/' + id, {
                type: 'GET',
                headers: {
                    "Authorization": "Bearer " + session.token
                }
            }).then(function (data) {
                resolve(data);
            }).fail(function (e) {
                reject(e);
            });
        });
    },
    /**
    * Resend confirmation code.
    */
    resend: function (id) {
        return new Promise(function (resolve, reject) {
            var session = SessionService.getSession();
            $.ajax(Constants.apiUrl + '/accounts/.grant/resend/' + id, {
                type: 'GET',
                headers: {
                    "Authorization": "Bearer " + session.token
                }
            }).then(function (data) {
                resolve(data);
            }).fail(function (e) {
                reject(e);
            });
        });
    },
    /**
    * Add the account.
    */
    add: function (request) {
        return new Promise(function (resolve, reject) {
            var data = JSON.stringify(request);
            var session = SessionService.getSession();
            $.ajax(Constants.apiUrl + '/accounts', {
                type: 'POST',
                contentType: 'application/json',
                data: data,
                headers: {
                    "Authorization": "Bearer " + session.token
                }
            }).then(function (data) {
                resolve(data);
            }).fail(function (e) {
                reject(e);
            });
        });
    }
};
import $ from 'jquery';

module.exports = {
    /* Get the session */
    getSession: function () {
        return new Promise(function (resolve, reject) {
            $.ajax('/Session', {
                type: 'GET'
            }).then(function (data) {
                resolve(data);
                }).fail(function (e) {
                    reject(e.responseJSON);
            });
        });
    },
    /* Create session */
    createSession: function (code, type) {
        return new Promise(function (resolve, reject) {
                var data = JSON.stringify({ pin_code: code, type: type });
                $.ajax('/Session', {
                    type: 'POST',
                    contentType: 'application/json',
                    data: data
                }).then(function (data) {
                    resolve(data);
                }).fail(function (e) {
                    reject(e.responseJSON);
                });
        });
    },
    /* Remove the session */
    removeSession: function () {
        return new Promise(function (resolve, reject) {
            $.ajax('/Session/Remove', {
                type: 'DELETE'
            }).then(function (data) {
                resolve(data);
            }).fail(function (e) {
                reject(e.responseJSON);
            });
        });
    }
};
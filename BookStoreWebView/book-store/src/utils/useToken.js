import React from "react";

export default function useToken() {
    const [token, setToken] = React.useState(getToken());
    const [authUserId, setAuthUserId] = React.useState(getUserId());

    function getToken() {
        return localStorage.getItem('refresh-token');
    }

    function saveToken({refreshToken}) {
        setToken(refreshToken);
        localStorage.setItem('refresh-token', refreshToken);
        setAuthUserId(JSON.parse(atob(refreshToken.split('.')[1])).nameid);
    }

    function removeToken() {
        setToken(null);
        localStorage.removeItem('refresh-token')
        setAuthUserId('')
    }

    function getUserId() {
        const tokenUserInfo = getToken()?.split('.')[1];
        if (tokenUserInfo) {
            return JSON.parse(atob(tokenUserInfo)).nameid;
        }

        return null;
    }

    return [token, saveToken, removeToken, authUserId]
}
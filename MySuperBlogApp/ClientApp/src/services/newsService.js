import {NEWS_URL, sendRequestWithToken } from "./commonService";

export async function getNewsByUser(userId) {
    const allNews = sendRequestWithToken(`${NEWS_URL}/${userId}`, 'GET');
    return allNews;
}
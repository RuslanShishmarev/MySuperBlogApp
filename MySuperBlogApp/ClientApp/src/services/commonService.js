const ACCOUNT_URL = 'account';
const USERS_URL = 'users';
const NEWS_URL = 'users';

const BASE_URL = 'login';

function sendRequest(url, successAction, errorAction) {
    fetch(url)
      .then(response => {
        if (response.status === 401) {
          window.location.href = BASE_URL;
        } else {
          // Обработка успешного ответа
          // ...
          successAction();
        }
      })
      .catch(error => {
        // Обработка ошибки запроса
        // ...
        errorAction();
      });
  }

  export async function getToken(login, password){
    const url = ACCOUNT_URL + '/token';
    const token = await sendAuthenticatedRequest(url, 'POST', login, password);
    console.log(token.accessToken);
  }

async function sendAuthenticatedRequest(url, method, username, password, data) {
    // Создаем объект заголовков
    var headers = new Headers();
  
    // Устанавливаем заголовок авторизации
    headers.set('Authorization', 'Basic ' + btoa(username + ':' + password));
  
    // Устанавливаем тип контента (если есть данные)
    if (data) {
      headers.set('Content-Type', 'application/json');
    }
  
    // Создаем объект параметров запроса
    var requestOptions = {
      method: method,
      headers: headers,
      body: data ? JSON.stringify(data) : undefined
    };
  
    // Отправляем запрос с помощью fetch
    var resultFetch = await fetch(url, requestOptions);
    if (resultFetch.ok) {
        const result = await resultFetch.json();
        return result;
    }
    else {
        // Произошла ошибка при выполнении запроса
        throw new Error('Ошибка ' + resultFetch.status + ': ' + resultFetch.statusText);
      }
  }
  
import { Counter } from "./components/Counter";
import { FetchData } from "./components/FetchData";
import { Home } from "./components/Home";
import Login from "./components/users/Login";
import SearchUser from "./components/users/SearchUser";
import SignUp from "./components/users/SignUp";
import UserProfile from "./components/users/UserProfile";
import UserPublicView from "./components/users/UserPublicView";
import { LOGIN_URL, PROFILE_URL, SIGNUP_URL, USERS_URL } from "./services/commonService";

const AppRoutes = [
  {
    index: true,
    element: <Home />
  },
  {
    path: '/counter',
    element: <Counter />
  },
  {
    path: '/fetch-data',
    element: <FetchData />
  },
  {
    path: LOGIN_URL,
    element: <Login />
  },
  {
    path: PROFILE_URL,
    element: <UserProfile />
  },
  {
    path: SIGNUP_URL,
    element: <SignUp />
  },
  {
    path: '/all',
    element: <SearchUser />
  },
  {
    path: '/all/:userId',
    element: <UserPublicView />
  }
];

export default AppRoutes;

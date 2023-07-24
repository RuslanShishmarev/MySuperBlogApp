import { Home } from "./components/Home";
import Login from "./components/users/Login";
import SearchUser from "./components/users/SearchUser";
import SignUp from "./components/users/SignUp";
import UserProfile from "./components/users/UserProfile";
import UserPublicView from "./components/users/UserPublicView";
import { ALLUSERS_URL, LOGIN_URL, PROFILE_URL, SIGNUP_URL, USERS_URL } from "./services/commonService";

const AppRoutes = [
  {
    index: true,
    element: <Home />
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
    path: ALLUSERS_URL,
    element: <SearchUser />
  },
  {
    path: `${ALLUSERS_URL}/:userId`,
    element: <UserPublicView />
  }
];

export default AppRoutes;

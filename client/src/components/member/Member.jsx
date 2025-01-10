import { NavLink, Outlet } from "react-router-dom";
import "../../assets/styles/Member.css";

export default function Member({ loggedInUser }) {
  return (
    <div className="member-layout border mt-5 w-100 px-0">
      {/* Member Header */}
      <div className="member-header">
        <h1>Welcome, {loggedInUser?.username || "Member"}!</h1>
        <p>Manage your account and navigate through the system below.</p>
      </div>

      {/* Navigation */}
      <nav className="d-flex justify-content-start member-nav">
        <div className="col-6 d-flex justify-content-end">
          <NavLink
            to="stats"
            className={({ isActive }) =>
              `mx-4 text-white text-decoration-none ${
                isActive ? "border-bottom border-danger border-5" : ""
              }`
            }
          >
            Stats
          </NavLink>
          <NavLink
            to="shop"
            className={({ isActive }) =>
              `me-4 text-white text-decoration-none ${
                isActive ? "border-bottom border-danger border-5" : ""
              }`
            }
          >
            Shop
          </NavLink>
          <NavLink
            to="tickets"
            className={({ isActive }) =>
              `text-white text-decoration-none me-4 ${
                isActive ? "border-bottom border-danger border-5" : ""
              }`
            }
          >
            Tickets
          </NavLink>
          <NavLink
            to="notifications"
            className={({ isActive }) =>
              `text-white text-decoration-none ${
                isActive ? "border-bottom border-danger border-5" : ""
              }`
            }
          >
            Notifications
          </NavLink>
          {/* Add more links here */}
        </div>
      </nav>

      {/* Dynamic Content */}
      <div className="member-content">
        <Outlet /> {/* This will render subroutes dynamically */}
      </div>
    </div>
  );
}

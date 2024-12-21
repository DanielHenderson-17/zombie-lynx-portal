import { useState, useEffect } from "react";
import { NavLink as RRNavLink } from "react-router-dom";
import { logout } from "../managers/authManager";
import { getUserProfiles } from "../managers/userProfileManager";
import "../assets/styles/NavBar.css";

export default function NavBar({ loggedInUser, setLoggedInUser }) {
  const [open, setOpen] = useState(false);
  const [userProfile, setUserProfile] = useState(null);

  useEffect(() => {
    if (loggedInUser) {
      console.log("Logged in user:", loggedInUser);
      getUserProfiles()
        .then((profiles) => {
          console.log("Fetched profiles:", profiles); // Debugging
          // Check if profiles is an array or an object
          if (Array.isArray(profiles)) {
            const profile = profiles.find(
              (p) =>
                p.identityUserId === loggedInUser.id || p.id === loggedInUser.id
            );
            console.log("Matched profile from array:", profile);
            setUserProfile(profile);
          } else {
            console.log("Fetched a single profile:", profiles);
            // If the API returns a single profile object
            if (
              profiles.identityUserId === loggedInUser.id ||
              profiles.id === loggedInUser.id
            ) {
              setUserProfile(profiles);
            } else {
              console.error("Profile does not match logged-in user");
            }
          }
        })
        .catch((error) =>
          console.error("Failed to fetch user profile:", error)
        );
    }
  }, [loggedInUser]);

  const toggleNavbar = () => setOpen(!open);

  return (
    <nav className="navbar navbar-expand-lg fixed-top p-0 mx-auto w-100 zlg-nav-bar">
      <div className="navbar-expand-lg navbar mx-auto col-6">
        <RRNavLink className="navbar-brand" to="/">
          <img className="zlg-logo" src="/zlg-logo.png" alt="" />
        </RRNavLink>
        {loggedInUser ? (
          <>
            <button
              className="navbar-toggler"
              type="button"
              aria-controls="navbarNav"
              aria-expanded={open}
              aria-label="Toggle navigation"
              onClick={toggleNavbar}
            >
              <span className="navbar-toggler-icon"></span>
            </button>
            <div className={`collapse navbar-collapse ${open ? "show" : ""}`}>
              <ul className="navbar-nav me-auto"></ul>
            </div>
            {userProfile ? (
              <span className="navbar-text me-3">
                {userProfile.firstName} {userProfile.lastName}
              </span>
            ) : (
              <span className="navbar-text me-3">Loading...</span>
            )}
            <button
              className="btn btn-primary"
              onClick={(e) => {
                e.preventDefault();
                setOpen(false);
                logout().then(() => {
                  setLoggedInUser(null);
                  setOpen(false);
                });
              }}
            >
              Logout
            </button>
          </>
        ) : (
          <div className="collapse navbar-collapse">
            <ul className="navbar-nav ms-auto">
              <li className="nav-item">
                <RRNavLink className="nav-link" to="/login">
                  <button className="btn btn-primary">Login</button>
                </RRNavLink>
              </li>
            </ul>
          </div>
        )}
      </div>
    </nav>
  );
}

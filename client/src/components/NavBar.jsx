import { useState, useEffect } from "react";
import { NavLink as RRNavLink } from "react-router-dom";
import { logout } from "../managers/authManager";
import { getUserProfiles } from "../managers/userProfileManager";
import "../assets/styles/NavBar.css";

export default function NavBar({ loggedInUser, setLoggedInUser }) {
  const [open, setOpen] = useState(false);
  const [userProfile, setUserProfile] = useState(null);

  // Fetches user profiles and sets the profile of the currently logged-in user based on matching identityUserId or id.
  useEffect(() => {
    if (loggedInUser) {
      getUserProfiles()
        .then((profiles) => {
          if (Array.isArray(profiles)) {
            const profile = profiles.find(
              (p) =>
                p.identityUserId === loggedInUser.id || p.id === loggedInUser.id
            );
            setUserProfile(profile);
          } else {
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

  // eslint-disable-next-line no-unused-vars
  const toggleNavbar = () => setOpen(!open);

  return (
    <nav className="navbar navbar-expand-lg fixed-top p-0 mx-auto w-100 zlg-nav-bar bg-dark">
      <div className="container d-flex justify-content-between align-items-center">
        {/* Logo */}
        <RRNavLink className="navbar-brand" to="/">
          <img
            className="zlg-logo"
            src="/zlg-logo.png"
            alt="Zombie Lynx Gaming"
          />
        </RRNavLink>

        {/* Links Section */}
        <div className="d-flex justify-content-center flex-grow-1"></div>

        {/* Email and Logout Section */}
        {loggedInUser && (
          <div className="d-flex align-items-center">
            {userProfile ? (
              <span className="navbar-text me-3 text-white">
                {userProfile.email}
              </span>
            ) : (
              <span className="navbar-text me-3 text-white">Loading...</span>
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
          </div>
        )}
      </div>
    </nav>
  );
}

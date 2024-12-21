import { Routes, Route, NavLink } from "react-router-dom";
import OpenTickets from "./OpenTickets";
// Placeholder for ClosedTickets
const ClosedTickets = () => <p>Closed Tickets Placeholder</p>;

export default function Tickets() {
  return (
    <div className="container tickets">
      <h1>Tickets</h1>
      {/* Component-Specific Navbar */}
      <ul className="nav nav-tabs mb-4 d-flex justify-content-center">
        <li className="nav-item">
          <NavLink
            to="/tickets/open-tickets"
            className={({ isActive }) => `nav-link ${isActive ? "active" : ""}`}
          >
            Open Tickets
          </NavLink>
        </li>
        <li className="nav-item">
          <NavLink
            to="/tickets/closed-tickets"
            className={({ isActive }) => `nav-link ${isActive ? "active" : ""}`}
          >
            Closed Tickets
          </NavLink>
        </li>
      </ul>

      {/* Nested Routes */}
      <Routes>
        <Route path="open-tickets" element={<OpenTickets />} />
        <Route path="closed-tickets" element={<ClosedTickets />} />
      </Routes>
    </div>
  );
}

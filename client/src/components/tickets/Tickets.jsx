import { Link, Route, Routes } from "react-router-dom";
import OpenTickets from "./OpenTickets";
import ClosedTickets from "./ClosedTickets";
import "../../assets/styles/tickets.css";

export default function Tickets() {
  return (
    <div className="d-flex justify-content-between ticket mt-5 ticket-container">
      {/* Sidebar for Navigation */}
      <div className="col-3 border p-3 ticket-nav">
        <div>
          <Link to="/tickets/open-tickets">
            <button className="btn d-block w-100 text-start mb-2 text-white">
              <i className="bi bi-inbox me-3"></i>Open Tickets
            </button>
          </Link>
          <hr />
          <Link to="/tickets/closed-tickets">
            <button className="btn d-block w-100 text-start text-white">
              <i className="bi bi-trash3 me-3"></i>Trash
            </button>
          </Link>
        </div>
      </div>

      {/* Main Content */}
      <div className="col-9 border ticket-body">
        <Routes>
          <Route path="open-tickets" element={<OpenTickets />} />
          <Route path="closed-tickets" element={<ClosedTickets />} />
        </Routes>
      </div>
    </div>
  );
}

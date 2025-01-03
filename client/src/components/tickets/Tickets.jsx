import { Link, Route, Routes } from "react-router-dom";
import OpenTickets from "./OpenTickets";
import ClosedTickets from "./ClosedTickets";
import NewTicket from "./NewTicket";
import "../../assets/styles/tickets.css";

export default function Tickets({ loggedInUser }) {
  return (
    <div className="d-flex justify-content-between ticket mt-5 ticket-container">
      {/* Sidebar for Navigation */}
      <div className="col-3 border p-3 ticket-nav">
        <div>
          {/* New Ticket Button */}
          <Link
            to="/tickets/new-ticket"
            className="d-flex justify-content-end text-decoration-none"
          >
            <button className="btn d-block w-25 text-start mb-3 btn-success">
              <i className="bi bi-plus-circle me-3"></i>Create
            </button>
          </Link>

          {/* Open Tickets Button */}
          <Link to="/tickets/open-tickets" className="text-decoration-none">
            <button className="btn d-block w-100 text-start mb-2 text-white">
              <i className="bi bi-inbox me-3"></i>Open Tickets
            </button>
          </Link>
          <hr />

          {/* Closed Tickets Button */}
          <Link to="/tickets/closed-tickets" className="text-decoration-none">
            <button className="btn d-block w-100 text-start text-white">
              <i className="bi bi-trash3 me-3"></i>Trash
            </button>
          </Link>
        </div>
      </div>

      {/* Main Content */}
      <div className="col-9 border ticket-body">
        <Routes>
          <Route
            path="new-ticket"
            element={<NewTicket loggedInUser={loggedInUser} />}
          />
          <Route path="open-tickets" element={<OpenTickets />} />
          <Route path="closed-tickets" element={<ClosedTickets />} />
        </Routes>
      </div>
    </div>
  );
}

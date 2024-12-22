import { useEffect, useState } from "react";
import {
  getOpenTickets,
  getClosedTickets,
  closeTicket,
  restoreTicket,
} from "../../managers/ticketManager";
import "../../assets/styles/tickets.css";

export default function TicketList() {
  const [tickets, setTickets] = useState([]);
  const [filter, setFilter] = useState("all");
  const [search, setSearch] = useState("");
  const [error, setError] = useState(null);

  useEffect(() => {
    Promise.all([getOpenTickets(), getClosedTickets()])
      .then(([open, closed]) => {
        setTickets([
          ...open.map((ticket) => ({ ...ticket, isOpen: true })),
          ...closed.map((ticket) => ({ ...ticket, isOpen: false })),
        ]);
      })
      .catch((error) => {
        console.error("Error fetching tickets:", error);
        setError("Failed to fetch tickets. Please try again.");
      });
  }, []);

  const handleCloseTicket = (ticketId) => {
    closeTicket(ticketId)
      .then(() => {
        setTickets((prevTickets) =>
          prevTickets.map((ticket) =>
            ticket.id === ticketId ? { ...ticket, isOpen: false } : ticket
          )
        );
      })
      .catch((error) => console.error("Error closing ticket:", error));
  };

  const handleReopenTicket = (ticketId) => {
    restoreTicket(ticketId)
      .then(() => {
        setTickets((prevTickets) =>
          prevTickets.map((ticket) =>
            ticket.id === ticketId ? { ...ticket, isOpen: true } : ticket
          )
        );
      })
      .catch((error) => console.error("Error restoring ticket:", error));
  };

  const filteredTickets = tickets
    .filter((ticket) => {
      if (filter === "open") return ticket.isOpen;
      if (filter === "closed") return !ticket.isOpen;
      return true;
    })
    .filter((ticket) =>
      ticket.subject.toLowerCase().includes(search.toLowerCase())
    );

  const openTickets = filteredTickets.filter((ticket) => ticket.isOpen);
  const closedTickets = filteredTickets.filter((ticket) => !ticket.isOpen);

  if (error) {
    return <p className="text-danger">{error}</p>;
  }

  return (
    <div>
      <div className="d-flex justify-content-between mb-3">
        <div>
          <button
            className={`btn ${
              filter === "all" ? "btn-primary" : "btn-outline-primary"
            } me-2`}
            onClick={() => setFilter("all")}
          >
            All
          </button>
          <button
            className={`btn ${
              filter === "open" ? "btn-primary" : "btn-outline-primary"
            } me-2`}
            onClick={() => setFilter("open")}
          >
            Open
          </button>
          <button
            className={`btn ${
              filter === "closed" ? "btn-primary" : "btn-outline-primary"
            }`}
            onClick={() => setFilter("closed")}
          >
            Closed
          </button>
        </div>
        <input
          type="text"
          className="form-control w-25"
          placeholder="Search by subject..."
          value={search}
          onChange={(e) => setSearch(e.target.value)}
        />
      </div>
      {openTickets.length > 0 && filter !== "closed" && (
        <>
          <h5>Open Tickets</h5>
          <table className="table table-striped align-middle">
            <thead className="thead-dark">
              <tr>
                <th className="text-start col-4">Topic</th>
                <th className="text-start col-2">Game</th>
                <th className="text-start col-2">Server</th>
                <th className="text-start col-1">Status</th>
                <th className="text-start col-1">Options</th>
              </tr>
            </thead>
            <tbody>
              {openTickets.map((ticket) => (
                <tr key={ticket.id}>
                  <td className="text-start col-4">
                    <div>
                      <strong>{ticket.subject}</strong>
                      <br />
                      <small className="text-muted">{ticket.category}</small>
                      <br />
                      <small className="text-muted">
                        {new Date(ticket.createdAt).toLocaleString()}
                      </small>
                      <br />
                      <small className="text-muted">
                        Assigned:{" "}
                        {ticket.assignedUsers
                          .map((user) => `${user.firstName} ${user.lastName}`)
                          .join(", ")}{" "}
                      </small>
                    </div>
                  </td>
                  <td className="text-start col-2">
                    <span className="text-warning fw-bold">{ticket.game}</span>
                  </td>
                  <td className="text-start col-2">
                    <span className="text-warning fw-bold">
                      {ticket.server}
                    </span>
                  </td>
                  <td className="text-start col-1">
                    <span className="text-warning fw-bold">
                      {ticket.status}
                    </span>
                  </td>
                  <td className="text-start col-1 position-relative">
                    <button
                      className="btn btn-danger btn-sm ticket-button"
                      onClick={() => handleCloseTicket(ticket.id)}
                    >
                      Close
                    </button>
                    <small className="position-absolute ticket-id">
                      Ticket ID: {ticket.id}
                    </small>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </>
      )}
      {closedTickets.length > 0 && filter !== "open" && (
        <>
          <h5>Closed Tickets</h5>
          <table className="table table-striped align-middle">
            <thead className="thead-dark">
              <tr>
                <th className="text-start col-4">Topic</th>
                <th className="text-start col-2">Game</th>
                <th className="text-start col-2">Server</th>
                <th className="text-start col-1">Status</th>
                <th className="text-start col-1">Options</th>
              </tr>
            </thead>
            <tbody>
              {closedTickets.map((ticket) => (
                <tr key={ticket.id}>
                  <td className="text-start col-4">
                    <div>
                      <strong>{ticket.subject}</strong>
                      <br />
                      <small className="text-muted">{ticket.category}</small>
                      <br />
                      <small className="text-muted">
                        {new Date(ticket.createdAt).toLocaleString()}
                      </small>
                      <br />
                      <small className="text-muted">
                        Assigned:{" "}
                        {ticket.assignedUsers
                          .map((user) => `${user.firstName} ${user.lastName}`)
                          .join(", ")}{" "}
                      </small>
                    </div>
                  </td>
                  <td className="text-start col-2">
                    <span className="text-warning fw-bold">{ticket.game}</span>
                  </td>
                  <td className="text-start col-2">
                    <span className="text-warning fw-bold">
                      {ticket.server}
                    </span>
                  </td>
                  <td className="text-start col-1">
                    <span className="text-success fw-bold">
                      {ticket.status}
                    </span>
                  </td>
                  <td className="text-start col-1 position-relative">
                    <button
                      className="btn btn-primary btn-sm ticket-button"
                      onClick={() => handleReopenTicket(ticket.id)}
                    >
                      Restore
                    </button>
                    <small className="position-absolute ticket-id">
                      Ticket ID: {ticket.id}
                    </small>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </>
      )}
    </div>
  );
}

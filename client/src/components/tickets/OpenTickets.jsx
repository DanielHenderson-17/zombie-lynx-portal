import { useEffect, useState } from "react";
import { getOpenTickets, closeTicketAPI } from "../../managers/ticketManager";

export default function OpenTickets() {
  const [tickets, setTickets] = useState([]);
  const [error, setError] = useState(null);

  useEffect(() => {
    getOpenTickets()
      .then((data) => {
        setTickets(data);
      })
      .catch((error) => {
        console.error("Error fetching open tickets:", error);
        setError("Failed to fetch open tickets. Please try again.");
      });
  }, []);

  const handleCloseTicket = (ticketId) => {
    closeTicketAPI(ticketId)
      .then(() => {
        setTickets((prevTickets) =>
          prevTickets.filter((ticket) => ticket.id !== ticketId)
        );
      })
      .catch((error) => console.error("Error closing ticket:", error));
  };

  if (error) {
    return <p className="text-danger">{error}</p>;
  }

  return (
    <div>
      <table className="table table-dark table-striped align-middle">
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
          {tickets.map((ticket) => (
            <tr key={ticket.id}>
              <td className="text-start col-4">
                <div>
                  <strong className="text-white">{ticket.subject}</strong>
                  <br />
                  <small className="sub-text">{ticket.category}</small>
                  <br />
                  <small className="sub-text">
                    {new Date(ticket.createdAt).toLocaleString()}
                  </small>
                  <br />
                  <small className="sub-text">
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
                <span className="text-warning fw-bold">{ticket.server}</span>
              </td>
              <td className="text-start col-1">
                <span className="text-warning fw-bold">{ticket.status}</span>
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
    </div>
  );
}

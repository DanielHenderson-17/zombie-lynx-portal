import { useEffect, useState } from "react";
import { getOpenTickets, closeTicket } from "../../managers/ticketManager";

export default function OpenTickets() {
  const [tickets, setTickets] = useState([]);
  const [error, setError] = useState(null);

  useEffect(() => {
    getOpenTickets()
      .then((data) => {
        console.log("Fetched open tickets:", data);
        setTickets(data);
      })
      .catch((error) => {
        console.error("Error fetching open tickets:", error);
        setError("Failed to fetch open tickets. Please try again.");
      });
  }, []);

  const handleCloseTicket = (ticketId) => {
    closeTicket(ticketId)
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
      <h1>Open Tickets</h1>
      {tickets.length > 0 ? (
        <table className="table table-striped align-middle">
          <thead className="thead-dark">
            <tr>
              <th className="text-start col-4">Topic</th>
              <th className="text-end col-2">Game</th>
              <th className="text-end col-2">Server</th>
              <th className="text-end col-1">Status</th>
              <th className="text-end col-1">Options</th>
            </tr>
          </thead>
          <tbody>
            {tickets.map((ticket) => (
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
                  </div>
                </td>
                <td className="text-end col-2">
                  <span className="text-warning fw-bold">{ticket.game}</span>
                </td>
                <td className="text-end col-2">
                  <span className="text-warning fw-bold">{ticket.server}</span>
                </td>
                <td className="text-end col-1">
                  <span className="text-warning fw-bold">{ticket.status}</span>
                </td>
                <td className="text-end col-1">
                  <button
                    className="btn btn-danger btn-sm"
                    onClick={() => handleCloseTicket(ticket.id)}
                  >
                    Close
                  </button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      ) : (
        <p>No open tickets available.</p>
      )}
    </div>
  );
}

import { useEffect, useState } from "react";
import {
  getClosedTickets,
  restoreTicketAPI,
  deleteTicket,
} from "../../managers/ticketManager";
import { truncateText } from "../../utils/truncateText";
import { getGameImage } from "../../utils/gameFormatter";

export default function ClosedTickets({ onTicketChange }) {
  // State to store closed tickets
  const [tickets, setTickets] = useState([]);

  // State to manage error messages
  const [error, setError] = useState(null);

  // Fetch closed tickets
  const fetchTickets = async () => {
    try {
      const data = await getClosedTickets();
      setTickets(data);
    } catch (error) {
      console.error("Error fetching closed tickets:", error);
      setError("Failed to fetch closed tickets. Please try again.");
    }
  };

  useEffect(() => {
    fetchTickets();
  }, []);

  // Handle restoring a ticket
  const handleRestoreTicket = async (ticketId) => {
    try {
      await restoreTicketAPI(ticketId);
      setTickets((prevTickets) =>
        prevTickets.filter((ticket) => ticket.id !== ticketId)
      );
      onTicketChange();
    } catch (error) {
      console.error("Error restoring ticket:", error);
    }
  };

  // Handle deleting a ticket
  const handleDeleteTicket = async (ticketId) => {
    try {
      await deleteTicket(ticketId);
      setTickets((prevTickets) =>
        prevTickets.filter((ticket) => ticket.id !== ticketId)
      );
    } catch (error) {
      console.error("Error deleting ticket:", error);
    }
  };

  if (error) {
    return <p className="text-danger">{error}</p>;
  }

  return (
    <div className="col-12 ticket-body">
      {tickets.length === 0 ? (
        <p className="mt-5 text-white">You have no tickets yet!</p>
      ) : (
        <table className="table table-dark table-striped align-middle">
          <thead className="thead-dark">
            <tr>
              <th className="text-start col-4">Topic</th>
              <th className="text-start col-1">Game</th>
              <th className="text-start col-2">Server</th>
              <th className="text-end col-2 pe-3">Options</th>
            </tr>
          </thead>
          <tbody>
            {tickets
              .slice()
              .reverse()
              .map((ticket) => (
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
                  <td className="text-start col-1">
                    <span className="text-warning fw-bold">
                      <img
                        className="gameImg ms-1"
                        src={getGameImage(ticket.game)}
                        alt=""
                      />
                    </span>
                  </td>
                  <td className="text-start col-2">
                    <span className="text-white fw-bold">
                      {truncateText(ticket.server)}
                    </span>
                  </td>
                  <td className="text-start col-2 position-relative">
                    <div className="d-flex justify-content-end pe-2">
                      <button
                        className="btn btn-primary btn-sm ticket-button me-2"
                        onClick={() => handleRestoreTicket(ticket.id)}
                      >
                        <i className="bi bi-arrow-counterclockwise"></i>
                      </button>
                      <button
                        className="btn btn-danger btn-sm ticket-button"
                        onClick={() => handleDeleteTicket(ticket.id)}
                      >
                        <i className="bi bi-trash3"></i>
                      </button>
                    </div>
                    <small className="position-absolute ticket-id">
                      Ticket ID: {ticket.id}
                    </small>
                  </td>
                </tr>
              ))}
          </tbody>
        </table>
      )}
    </div>
  );
}

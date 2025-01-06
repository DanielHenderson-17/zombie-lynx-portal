export const truncateText = (text, maxLength = 30) => {
  return text.length <= maxLength ? text : `${text.slice(0, maxLength)}...`;
};
